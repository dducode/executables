using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Extensions;
using Interactions.Core.Queries;
using Interactions.Policies;
using Interactions.Queries;

namespace Interactions.Extensions;

public static class QueriesExtensions {

  [Pure]
  public static IQuery<T1, T3> Chain<T1, T2, T3>(this IQuery<T1, T2> first, IQuery<T2, T3> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new ChainedQuery<T1, T2, T3>(first, second);
  }

  [Pure]
  public static IAsyncQuery<T1, T3> Chain<T1, T2, T3>(this IAsyncQuery<T1, T2> first, IAsyncQuery<T2, T3> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new AsyncChainedQuery<T1, T2, T3>(first, second);
  }

  [Pure]
  public static IAsyncQuery<T1, T3> Chain<T1, T2, T3>(this IAsyncQuery<T1, T2> first, IQuery<T2, T3> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return first.Chain(second.ToAsyncQuery());
  }

  [Pure]
  public static IAsyncQuery<T1, T3> Chain<T1, T2, T3>(this IQuery<T1, T2> first, IAsyncQuery<T2, T3> second) {
    return first.ToAsyncQuery().Chain(second);
  }

  [Pure]
  public static IQuery<T1, T2> WithPolicy<T1, T2>(this IQuery<T1, T2> query, Policy<T1, T2> policy) {
    ExceptionsHelper.ThrowIfNull(policy, nameof(policy));
    return new PolicyQuery<T1, T2>(query, policy);
  }

  [Pure]
  public static IAsyncQuery<T1, T2> WithPolicy<T1, T2>(this IAsyncQuery<T1, T2> query, AsyncPolicy<T1, T2> policy) {
    ExceptionsHelper.ThrowIfNull(policy, nameof(policy));
    return new AsyncPolicyQuery<T1, T2>(query, policy);
  }

}