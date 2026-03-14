using Interactions.Core;
using Interactions.Fallbacks;

namespace Interactions.Policies;

internal sealed class AsyncFallbackPolicy<T1, T2, TEx>(IFallbackHandler<T1, TEx, T2> handler) : AsyncPolicy<T1, T2> where TEx : Exception {

  public override async ValueTask<T2> Invoke(T1 input, IAsyncExecutor<T1, T2> executor, CancellationToken token = default) {
    token.ThrowIfCancellationRequested();

    try {
      return await executor.Execute(input, token);
    }
    catch (TEx e) {
      return handler.Fallback(input, e);
    }
  }

}