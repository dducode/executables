using System.Diagnostics;
using Interactions.Core;

namespace Interactions.Analytics;

[Obsolete]
internal sealed class MetricsHandler<T1, T2>(Handler<T1, T2> inner, IMetrics<T1, T2> metrics, string tag) : Handler<T1, T2> {

  private readonly Stopwatch _sw = new();

  protected override T2 HandleCore(T1 input) {
    _sw.Restart();
    metrics.Call(tag, input);

    try {
      T2 result = inner.Handle(input);
      metrics.Success(tag, result);
      return result;
    }
    catch (Exception e) {
      metrics.Failure(tag, e);
      throw;
    }
    finally {
      _sw.Stop();
      metrics.Latency(tag, _sw.Elapsed);
    }
  }

  protected override void DisposeCore() {
    inner.Dispose();
  }

}