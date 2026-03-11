using System.Diagnostics.Contracts;
using Interactions.Analytics;
using Interactions.Core.Internal;
using Interactions.Operations;
using Interactions.Policies;

namespace Interactions;

public static class ExecutionOperator {

  [Pure]
  public static ExecutionOperator<T1, T1, T2, T2> Identity<T1, T2>() {
    return IdentityOperator<T1, T2>.Instance;
  }

  [Pure]
  public static BehaviorOperator<T1, T2> Cache<T1, T2>(ICacheStorage<T1, T2> storage) {
    ExceptionsHelper.ThrowIfNull(storage, nameof(storage));
    return new CacheOperator<T1, T2>(storage);
  }

  [Pure]
  public static BehaviorOperator<T1, T2> Metrics<T1, T2>(this IMetrics<T1, T2> metrics, string tag = null) {
    ExceptionsHelper.ThrowIfNull(metrics, nameof(metrics));
    return new MetricsOperator<T1, T2>(metrics, tag);
  }

  [Pure]
  public static ExecutionOperator<T1, T2, T3, T4> Create<T1, T2, T3, T4>(ExecutionFunc<T1, T2, T3, T4> operation) {
    ExceptionsHelper.ThrowIfNull(operation, nameof(operation));
    return new AnonymousOperator<T1, T2, T3, T4>(operation);
  }

}