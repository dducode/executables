using Interactions.Core.Queries;

namespace Interactions.Queries;

internal sealed class ChainedQuery<T1, T2, T3>(IQuery<T1, T2> first, IQuery<T2, T3> second) : IQuery<T1, T3> {

  public T3 Send(T1 input) {
    return second.Send(first.Send(input));
  }

}

internal sealed class AsyncChainedQuery<T1, T2, T3>(IAsyncQuery<T1, T2> first, IAsyncQuery<T2, T3> second) : IAsyncQuery<T1, T3> {

  public async ValueTask<T3> Send(T1 input, CancellationToken token = default) {
    return await second.Send(await first.Send(input, token), token);
  }

}