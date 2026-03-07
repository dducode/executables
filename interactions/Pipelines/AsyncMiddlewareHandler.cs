using Interactions.Core;
using Interactions.Core.Handlers;

namespace Interactions.Pipelines;

internal sealed class AsyncMiddlewareHandler<T1, T2, T3, T4>(AsyncMiddleware<T1, T2, T3, T4> middleware, AsyncHandler<T2, T3> next) : AsyncHandler<T1, T4> {

  protected override ValueTask<T4> ExecuteCore(T1 input, CancellationToken token = default) {
    return middleware.Invoke(input, next, token);
  }

  protected override void DisposeCore() {
    next.Dispose();
  }

}