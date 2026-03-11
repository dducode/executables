using System.Collections.Concurrent;
using System.Diagnostics;
using Interactions.Analytics;
using Interactions.Core;

namespace Interactions.Operations;

internal sealed class AsyncMetricsOperator<T1, T2>(IMetrics<T1, T2> metrics, string tag) : AsyncBehaviorOperator<T1, T2> {

  private readonly ConcurrentStack<Stopwatch> _stopwatches = new();

  public override async ValueTask<T2> Invoke(T1 input, IAsyncExecutable<T1, T2> next, CancellationToken token = default) {
    if (!_stopwatches.TryPop(out Stopwatch sw))
      sw = new Stopwatch();

    try {
      metrics.Call(tag, input);
      T2 result;

      try {
        sw.Start();
        result = await next.Execute(input, token);
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
      sw.Reset();
      _stopwatches.Push(sw);
    }
  }

}