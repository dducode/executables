using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions;

public static class AsyncMap {

  [Pure]
  public static AsyncMap<T1, T1, T2, T2> Identity<T1, T2>() {
    return AsyncMap<T1, T2, T1, T2>.Identity;
  }

  [Pure]
  public static AsyncMap<T1, T2, T3, T4> Create<T1, T2, T3, T4>(Transformer<T1, T2> incoming, Transformer<T3, T4> outgoing) {
    ExceptionsHelper.ThrowIfNull(incoming, nameof(incoming));
    ExceptionsHelper.ThrowIfNull(outgoing, nameof(outgoing));
    return new AsyncMap<T1, T2, T3, T4>(incoming, outgoing);
  }

  [Pure]
  public static AsyncMap<T1, T2, T3, T4> Create<T1, T2, T3, T4>(Func<T1, T2> incoming, Func<T3, T4> outgoing) {
    return Create(Transformer.Create(incoming), Transformer.Create(outgoing));
  }

}