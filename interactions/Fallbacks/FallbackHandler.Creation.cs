using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Fallbacks;

public static class FallbackHandler {

  [Pure]
  public static IFallbackHandler<T1, TException, T2> Create<T1, TException, T2>(Func<T1, TException, T2> fallback) where TException : Exception {
    ExceptionsHelper.ThrowIfNull(fallback, nameof(fallback));
    return new AnonymousFallbackHandler<T1, TException, T2>(fallback);
  }

}