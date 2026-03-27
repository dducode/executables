namespace Interactions.Core.Executables;

internal sealed class AsyncCompositeExecutable<T1, T2, T3>(IAsyncExecutable<T1, T2> first, IAsyncExecutable<T2, T3> second)
  : IAsyncExecutable<T1, T3>, IAsyncExecutor<T1, T3> {

  private readonly IAsyncExecutor<T1, T2> _first = first.GetExecutor();
  private readonly IAsyncExecutor<T2, T3> _second = second.GetExecutor();

  IAsyncExecutor<T1, T3> IAsyncExecutable<T1, T3>.GetExecutor() {
    return this;
  }

  ValueTask<T3> IAsyncExecutor<T1, T3>.Execute(T1 input, CancellationToken token) {
    ValueTask<T2> firstTask = _first.Execute(input, token);
    if (firstTask.IsCompleted)
      return _second.Execute(firstTask.Result, token);

    return Await(firstTask, _second, token);
  }

  private static async ValueTask<T3> Await(ValueTask<T2> task, IAsyncExecutor<T2, T3> second, CancellationToken token) {
    return await second.Execute(await task, token);
  }

}