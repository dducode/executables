using Interactions.Core;

namespace Interactions.Pipelines;

internal sealed class MiddlewareHandler<T1, T2, T3, T4>(Middleware<T1, T2, T3, T4> middleware, Handler<T2, T3> next) : Handler<T1, T4> {

  protected override T4 HandleCore(T1 input) {
    return middleware.Invoke(input, next);
  }

  protected override void DisposeCore() {
    next.Dispose();
  }

}