using Interactions.Core;

namespace Interactions.Executables;

internal sealed class AsyncFlowExecutable<T1, T2, T3, T4>(
  IAsyncExecutable<T1, T2> first,
  IAsyncExecutable<T1, T3> second,
  IAggregator<T2, T3, T4> aggregator) : IAsyncExecutable<T1, T4> {

  public async ValueTask<T4> Execute(T1 input, CancellationToken token = default) {
    Task<T2> t1 = first.Execute(input, token).AsTask();
    Task<T3> t2 = second.Execute(input, token).AsTask();
    await Task.WhenAll(t1, t2);
    return aggregator.Aggregate(t1.Result, t2.Result);
  }

}