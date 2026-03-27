using System.Diagnostics.Contracts;
using Interactions.Core.Analytics;
using Interactions.Internal;

namespace Interactions.Analytics;

public static class Metrics {

  [Pure]
  public static IMetrics<T1, T2> Create<T1, T2>(Action<T1> call, Action<T2> success, Action<Exception> failure = null, Action<TimeSpan> latency = null) {
    ExceptionsHelper.ThrowIfNull(call, nameof(call));
    ExceptionsHelper.ThrowIfNull(success, nameof(success));
    return new AnonymousMetrics<T1, T2>(call, success, failure, latency);
  }

}