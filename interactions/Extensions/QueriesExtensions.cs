using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Extensions;
using Interactions.Core.Queries;
using Interactions.Queries;

namespace Interactions.Extensions;

public static class QueriesExtensions {

  [Pure]
  public static Query<T1, T3> Chain<T1, T2, T3>(this Query<T1, T2> first, Query<T2, T3> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new ChainedQuery<T1, T2, T3>(first, second);
  }

  [Pure]
  public static AsyncQuery<T1, T3> Chain<T1, T2, T3>(this AsyncQuery<T1, T2> first, AsyncQuery<T2, T3> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new AsyncChainedQuery<T1, T2, T3>(first, second);
  }

  [Pure]
  public static AsyncQuery<T1, T3> Chain<T1, T2, T3>(this AsyncQuery<T1, T2> first, Query<T2, T3> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return first.Chain(second.ToAsyncQuery());
  }

  [Pure]
  public static AsyncQuery<T1, T3> Chain<T1, T2, T3>(this Query<T1, T2> first, AsyncQuery<T2, T3> second) {
    return first.ToAsyncQuery().Chain(second);
  }

}