using Interactions.Fallbacks;

namespace Interactions.Core.Fallbacks;

internal sealed class AnonymousFallbackHandler<T1, TException, T2>(Func<T1, TException, T2> fallback)
  : IFallbackHandler<T1, TException, T2> where TException : Exception {

  public T2 Fallback(T1 input, TException e) {
    return fallback(input, e);
  }

}