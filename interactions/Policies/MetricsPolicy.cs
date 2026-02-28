using System.Collections.Concurrent;
using System.Diagnostics;
using Interactions.Analytics;

namespace Interactions.Policies;

internal sealed class MetricsPolicy<T1, T2>(IMetrics<T1, T2> metrics, string tag) : Policy<T1, T2> {

  private readonly ConcurrentStack<Stopwatch> _stopwatches = new();

  public override T2 Execute(T1 input, Func<T1, T2> invocation) {
    if (!_stopwatches.TryPop(out Stopwatch sw))
      sw = new Stopwatch();

    try {
      metrics.Call(tag, input);
      T2 result;

      try {
        sw.Restart();
        result = invocation.Invoke(input);
        sw.Stop();
      }
      catch (Exception e) {
        sw.Stop();
        metrics.Failure(tag, e);
        throw;
      }
      finally {
        metrics.Latency(tag, sw.Elapsed);
      }

      metrics.Success(tag, result);
      return result;
    }
    finally {
      _stopwatches.Push(sw);
    }
  }

}