using Interactions.Core;
using Interactions.Fallbacks;

namespace Interactions.Policies;

internal sealed class FallbackPolicy<T1, T2, TException>(IFallbackHandler<T1, TException, T2> handler) : Policy<T1, T2> where TException : Exception {

  public override T2 Invoke(T1 input, IExecutable<T1, T2> executable) {
    try {
      return executable.Execute(input);
    }
    catch (TException e) {
      return handler.Fallback(input, e);
    }
  }

}