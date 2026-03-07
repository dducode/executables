using Interactions.Core;
using Interactions.Fallbacks;

namespace Interactions.Policies;

internal sealed class AsyncFallbackPolicy<T1, T2, TException>(IFallbackHandler<T1, TException, T2> handler) : AsyncPolicy<T1, T2> where TException : Exception {

  public override async ValueTask<T2> Invoke(T1 input, IAsyncExecutable<T1, T2> executable, CancellationToken token = default) {
    token.ThrowIfCancellationRequested();

    try {
      return await executable.Execute(input, token);
    }
    catch (TException e) {
      return handler.Fallback(input, e);
    }
  }

}