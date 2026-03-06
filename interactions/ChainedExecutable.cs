using Interactions.Core;

namespace Interactions;

internal sealed class ChainedExecutable<T1, T2, T3>(IExecutable<T1, T2> first, IExecutable<T2, T3> second) : IExecutable<T1, T3> {

  public T3 Execute(T1 input) {
    return second.Execute(first.Execute(input));
  }

}

internal sealed class AsyncChainedExecutable<T1, T2, T3>(IAsyncExecutable<T1, T2> first, IAsyncExecutable<T2, T3> second) : IAsyncExecutable<T1, T3> {

  public async ValueTask<T3> Execute(T1 input, CancellationToken token = default) {
    return await second.Execute(await first.Execute(input, token), token);
  }

}