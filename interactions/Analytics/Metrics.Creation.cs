using System.Diagnostics.Contracts;

namespace Interactions.Analytics;

public static class Metrics {

  [Pure]
  public static IMetrics<T1, T2> Create<T1, T2>(
    Action<T1> call = null, Action<T2> success = null, Action<Exception> failure = null, Action<TimeSpan> latency = null) {
    return new AnonymousMetrics<T1, T2>(call, success, failure, latency);
  }

}