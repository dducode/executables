using System.Collections.Concurrent;
using System.Diagnostics;
using Interactions.Analytics;
using Interactions.Operations;

namespace Interactions.Core.Operators;

internal sealed class AsyncMetricsOperator<T1, T2>(IMetrics<T1, T2> metrics, string tag) : AsyncBehaviorOperator<T1, T2> {

  private readonly ConcurrentStack<Stopwatch> _stopwatches = new();

  public override async ValueTask<T2> Invoke(T1 input, IAsyncExecutor<T1, T2> executor, CancellationToken token = default) {
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