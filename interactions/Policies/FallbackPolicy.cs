using Interactions.Core;
using Interactions.Fallbacks;

namespace Interactions.Policies;

internal sealed class FallbackPolicy<T1, T2, TException>(IFallbackHandler<T1, TException, T2> handler) : Policy<T1, T2> where TException : Exception {

  public override T2 Execute(T1 input, Func<T1, T2> invocation) {
    try {
      return invocation(input);
    }
    catch (TException e) {
      return handler.Fallback(input, e);
    }
  }

}

internal sealed class AsyncFallbackPolicy<T1, T2, TException>(IFallbackHandler<T1, TException, T2> handler) : AsyncPolicy<T1, T2> where TException : Exception {

  public override async ValueTask<T2> Execute(T1 input, AsyncFunc<T1, T2> invocation, CancellationToken token) {
    token.ThrowIfCancellationRequested();

    try {
      return await invocation(input, token);
    }
    catch (TException e) {
      return handler.Fallback(input, e);
    }
  }

}