using System.Diagnostics.Contracts;

namespace Interactions.Analytics;

public interface IMetrics<in T1, in T2> {

  void Call(string tag, T1 input);
  void Success(string tag, T2 output);
  void Failure(string tag, Exception exception);
  void Latency(string tag, TimeSpan duration);

}

public static class Metrics {

  [Pure]
  public static IMetrics<T1, T2> Create<T1, T2>(
    Action<T1> call = null, Action<T2> success = null, Action<Exception> failure = null, Action<TimeSpan> latency = null) {
    return new AnonymousMetrics<T1, T2>(call, success, failure, latency);
  }

}