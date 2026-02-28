using Interactions.Core;

namespace Interactions.Pipelines;

internal sealed class AsyncMiddlewareHandler<T1, T2, T3, T4>(AsyncMiddleware<T1, T2, T3, T4> middleware, AsyncHandler<T2, T3> next) : AsyncHandler<T1, T4> {

  public override ValueTask<T4> Handle(T1 input, CancellationToken token = default) {
    ThrowIfDisposed(nameof(AsyncMiddlewareHandler<T1, T2, T3, T4>));
    return middleware.Invoke(input, next, token);
  }

  protected override void DisposeCore() {
    next.Dispose();
  }

}