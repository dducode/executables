using Interactions.Internal;

namespace Interactions.Core.Executables;

internal sealed class RaceExecutable<T1, T2> : IAsyncExecutable<T1, T2>, IAsyncExecutor<T1, T2> {

  private readonly IAsyncExecutor<T1, T2>[] _executors;

  internal RaceExecutable(IAsyncExecutable<T1, T2>[] executables) {
    _executors = new IAsyncExecutor<T1, T2>[executables.Length];
    for (var i = 0; i < executables.Length; i++)
      _executors[i] = executables[i].GetExecutor();
  }

  IAsyncExecutor<T1, T2> IAsyncExecutable<T1, T2>.GetExecutor() {
    return this;
  }

  ValueTask<T2> IAsyncExecutor<T1, T2>.Execute(T1 input, CancellationToken token) {
    token.ThrowIfCancellationRequested();

    List<ValueTask<T2>> valueTasks = Pool<List<ValueTask<T2>>>.Get();
    using var valueTasksHandle = new ListHandle<ValueTask<T2>>(valueTasks);

    for (var i = 0; i < _executors.Length; i++) {
      ValueTask<T2> valueTask = _executors[i].Execute(input, token);
      if (valueTask.IsCompleted)
        return new ValueTask<T2>(valueTask.Result);
      valueTasks.Add(valueTask);
    }

    return Await(valueTasks);
  }

  private static async ValueTask<T2> Await(List<ValueTask<T2>> valueTasks) {
    List<Task<T2>> tasks = Pool<List<Task<T2>>>.Get();
    using var tasksHandle = new ListHandle<Task<T2>>(tasks);

    for (var i = 0; i < valueTasks.Count; i++)
      tasks.Add(valueTasks[i].AsTask());

    Task<T2> winner = await Task.WhenAny(tasks);
    return await winner;
  }

}