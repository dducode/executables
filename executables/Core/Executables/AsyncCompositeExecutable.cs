namespace Executables.Core.Executables;

internal sealed class AsyncCompositeExecutable<T1, T2, T3>(IAsyncExecutable<T2, T3> first, IAsyncExecutable<T1, T2> second)
  : IAsyncExecutable<T1, T3>, IAsyncExecutor<T1, T3> {

  private readonly IAsyncExecutor<T2, T3> _first = first.GetExecutor();
  private readonly IAsyncExecutor<T1, T2> _second = second.GetExecutor();

  IAsyncExecutor<T1, T3> IAsyncExecutable<T1, T3>.GetExecutor() {
    return this;
  }

  ValueTask<T3> IAsyncExecutor<T1, T3>.Execute(T1 input, CancellationToken token) {
    ValueTask<T2> firstTask = _second.Execute(input, token);
    if (firstTask.IsCompleted)
      return _first.Execute(firstTask.Result, token);

    return Await(firstTask, _first, token);
  }

  private static async ValueTask<T3> Await(ValueTask<T2> task, IAsyncExecutor<T2, T3> first, CancellationToken token) {
    return await first.Execute(await task, token);
  }

}