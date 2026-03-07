using System.Diagnostics.Contracts;
using Interactions.Core;

namespace Interactions.Fallbacks;

public interface IFallbackHandler<in T1, in TException, out T2> where TException : Exception {

  T2 Fallback(T1 input, TException e);

}

public static class FallbackHandler {

  [Pure]
  public static IFallbackHandler<T1, TException, T2> Create<T1, TException, T2>(Func<T1, TException, T2> fallback) where TException : Exception {
    ExceptionsHelper.ThrowIfNull(fallback, nameof(fallback));
    return new AnonymousFallbackHandler<T1, TException, T2>(fallback);
  }

}