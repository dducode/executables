using System.Diagnostics.Contracts;
using Interactions.Analytics;
using Interactions.Core.Internal;
using Interactions.Operations;
using Interactions.Policies;

namespace Interactions;

public static class AsyncExecutionOperator {

  [Pure]
  public static AsyncBehaviorOperator<T1, T2> Memoize<T1, T2>(ICacheStorage<T1, T2> storage) {
    ExceptionsHelper.ThrowIfNull(storage, nameof(storage));
    return new AsyncMemoizationOperator<T1, T2>(storage);
  }

  [Pure]
  public static AsyncBehaviorOperator<T1, T2> Metrics<T1, T2>(this IMetrics<T1, T2> metrics, string tag = null) {
    ExceptionsHelper.ThrowIfNull(metrics, nameof(metrics));
    return new AsyncMetricsOperator<T1, T2>(metrics, tag);
  }

  [Pure]
  public static AsyncExecutionOperator<T1, T2, T3, T4> Create<T1, T2, T3, T4>(AsyncExecutionFunc<T1, T2, T3, T4> operation) {
    ExceptionsHelper.ThrowIfNull(operation, nameof(operation));
    return new AsyncAnonymousOperator<T1, T2, T3, T4>(operation);
  }

}