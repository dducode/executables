using System.Collections.Concurrent;
using System.Diagnostics;
using Executables.Analytics;

namespace Executables.Core.Executors;

internal sealed class MetricsExecutor<T1, T2>(IExecutor<T1, T2> executor, IMetrics<T1, T2> metrics, string tag) : IExecutor<T1, T2> {

  private readonly ConcurrentStack<Stopwatch> _stopwatches = new();

  T2 IExecutor<T1, T2>.Execute(T1 input) {
    if (!_stopwatches.TryPop(out Stopwatch sw))
      sw = new Stopwatch();

    try {
      metrics.Call(tag, input);
      T2 result;

      try {
        sw.Start();
        result = executor.Execute(input);
        sw.Stop();
      }
      catch (Exception e) {
        sw.Stop();
        metrics.Failure(tag, e);
        metrics.Latency(tag, sw.Elapsed);
        throw;
      }

      metrics.Success(tag, result);
      metrics.Latency(tag, sw.Elapsed);
      return result;
    }
    finally {
      sw.Reset();
      _stopwatches.Push(sw);
    }
  }

}