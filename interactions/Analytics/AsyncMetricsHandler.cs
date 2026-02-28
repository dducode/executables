using System.Diagnostics;
using Interactions.Core;

namespace Interactions.Analytics;

[Obsolete]
internal sealed class AsyncMetricsHandler<T1, T2>(AsyncHandler<T1, T2> inner, IMetrics<T1, T2> metrics, string tag) : AsyncHandler<T1, T2> {

  private readonly Stopwatch _sw = new();

  public override async ValueTask<T2> Handle(T1 input, CancellationToken token = default) {
    ThrowIfDisposed(nameof(AsyncMetricsHandler<T1, T2>));

    _sw.Restart();
    metrics.Call(tag, input);

    try {
      T2 result = await inner.Handle(input, token);
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