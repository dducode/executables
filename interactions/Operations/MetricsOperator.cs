using System.Collections.Concurrent;
using System.Diagnostics;
using Interactions.Analytics;
using Interactions.Core;

namespace Interactions.Operations;

internal sealed class MetricsOperator<T1, T2>(IMetrics<T1, T2> metrics, string tag) : BehaviorOperator<T1, T2> {

  private readonly ConcurrentStack<Stopwatch> _stopwatches = new();

  public override T2 Invoke(T1 input, IExecutable<T1, T2> next) {
    if (!_stopwatches.TryPop(out Stopwatch sw))
      sw = new Stopwatch();

    try {
      metrics.Call(tag, input);
      T2 result;

      try {
        sw.Start();
        result = next.Execute(input);
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