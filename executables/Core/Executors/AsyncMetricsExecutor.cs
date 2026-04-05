using System.Collections.Concurrent;
using System.Diagnostics;
using Executables.Analytics;

namespace Executables.Core.Executors;

internal sealed class AsyncMetricsExecutor<T1, T2>(IAsyncExecutor<T1, T2> executor, IMetrics<T1, T2> metrics, string tag) : IAsyncExecutor<T1, T2> {

  private readonly ConcurrentStack<Stopwatch> _stopwatches = new();

  async ValueTask<T2> IAsyncExecutor<T1, T2>.Execute(T1 input, CancellationToken token) {
    if (!_stopwatches.TryPop(out Stopwatch sw))
      sw = new Stopwatch();

    try {
      metrics.Call(tag, input);
      T2 result;

      try {
        sw.Start();
        result = await executor.Execute(input, token);
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