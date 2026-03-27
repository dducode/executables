using Interactions.Internal;

namespace Interactions.Core.Executables;

internal sealed class RaceSuccessExecutable<T1, T2> : IAsyncExecutable<T1, T2>, IAsyncExecutor<T1, T2> {

  private readonly IAsyncExecutor<T1, T2>[] _executors;

  internal RaceSuccessExecutable(IAsyncExecutable<T1, T2>[] executables) {
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

    List<Exception> exceptions = Pool<List<Exception>>.Get();
    using var exceptionsHandle = new ListHandle<Exception>(exceptions);

    for (var i = 0; i < _executors.Length; i++) {
      ValueTask<T2> valueTask;

      try {
        valueTask = _executors[i].Execute(input, token);
        if (valueTask.IsCompleted)
          return new ValueTask<T2>(valueTask.Result);
      }
      catch (Exception e) {
        exceptions.Add(e);
        continue;
      }

      valueTasks.Add(valueTask);
    }

    return valueTasks.Count switch {
      0 => exceptions.All(e => e is OperationCanceledException) ? throw new OperationCanceledException(token) : throw new AggregateException(exceptions),
      1 => Await(valueTasks[0], exceptions, token),
      _ => Await(valueTasks, exceptions, token)
    };
  }

  private static async ValueTask<T2> Await(ValueTask<T2> task, List<Exception> otherExceptions, CancellationToken token) {
    List<Exception> exceptions = Pool<List<Exception>>.Get();
    using var exceptionsHandle = new ListHandle<Exception>(exceptions);
    exceptions.AddRange(otherExceptions);

    try {
      return await task;
    }
    catch (Exception exception) {
      exceptions.Add(exception);
      if (exceptions.All(e => e is OperationCanceledException))
        throw new OperationCanceledException(token);
      throw new AggregateException(exceptions);
    }
  }

  private static async ValueTask<T2> Await(List<ValueTask<T2>> valueTasks, List<Exception> otherExceptions, CancellationToken token) {
    List<Exception> exceptions = Pool<List<Exception>>.Get();
    using var exceptionsHandle = new ListHandle<Exception>(exceptions);
    exceptions.AddRange(otherExceptions);

    List<Task<T2>> tasks = Pool<List<Task<T2>>>.Get();
    using var tasksHandle = new ListHandle<Task<T2>>(tasks);

    for (var i = 0; i < valueTasks.Count; i++)
      tasks.Add(valueTasks[i].AsTask());

    while (tasks.Count > 0) {
      Task<T2> winner = await Task.WhenAny(tasks);
      tasks.Remove(winner);

      try {
        return await winner;
      }
      catch (Exception e) {
        exceptions.Add(e);
      }
    }

    if (exceptions.All(e => e is OperationCanceledException))
      throw new OperationCanceledException(token);
    throw new AggregateException(exceptions);
  }

}