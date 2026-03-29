namespace Interactions.Core.Queries;

internal sealed class AsyncChainedQuery<T1, T2, T3>(IAsyncQuery<T1, T2> first, IAsyncQuery<T2, T3> second) : IAsyncQuery<T1, T3>, IAsyncExecutor<T1, T3> {

  public ValueTask<T3> Send(T1 input, CancellationToken token = default) {
    token.ThrowIfCancellationRequested();
    ValueTask<T2> firstTask = first.Send(input, token);

    if (firstTask.IsCompleted)
      return second.Send(firstTask.Result, token);

    return Await(firstTask, second, token);
  }

  IAsyncExecutor<T1, T3> IAsyncExecutable<T1, T3>.GetExecutor() {
    return this;
  }

  ValueTask<T3> IAsyncExecutor<T1, T3>.Execute(T1 input, CancellationToken token) {
    return Send(input, token);
  }

  private static async ValueTask<T3> Await(ValueTask<T2> first, IAsyncQuery<T2, T3> second, CancellationToken token) {
    return await second.Send(await first, token);
  }

}