using Interactions.Core;

namespace Interactions.Pipelines;

internal sealed class MiddlewareHandler<T1, T2, T3, T4>(Middleware<T1, T2, T3, T4> middleware, Handler<T2, T3> next) : Handler<T1, T4> {

  public override T4 Handle(T1 input) {
    ThrowIfDisposed(nameof(MiddlewareHandler<T1, T2, T3, T4>));
    return middleware.Invoke(input, next);
  }

  protected override void DisposeCore() {
    next.Dispose();
  }

}