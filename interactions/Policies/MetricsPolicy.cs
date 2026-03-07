using System.Collections.Concurrent;
using System.Diagnostics;
using Interactions.Analytics;
using Interactions.Core.Executables;

namespace Interactions.Policies;

internal sealed class MetricsPolicy<T1, T2>(IMetrics<T1, T2> metrics, string tag) : Policy<T1, T2> {

  private readonly ConcurrentStack<Stopwatch> _stopwatches = new();

  public override T2 Invoke(T1 input, IExecutable<T1, T2> executable) {
    if (!_stopwatches.TryPop(out Stopwatch sw))
      sw = new Stopwatch();

    try {
      metrics.Call(tag, input);
      T2 result;

      try {
        sw.Restart();
        result = executable.Execute(input);
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