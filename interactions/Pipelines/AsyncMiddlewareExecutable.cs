using Interactions.Core;

namespace Interactions.Pipelines;

internal sealed class AsyncMiddlewareExecutable<T1, T2, T3, T4>(AsyncMiddleware<T1, T2, T3, T4> middleware, IAsyncExecutable<T2, T3> next)
  : IAsyncExecutable<T1, T4> {

  public ValueTask<T4> Execute(T1 input, CancellationToken token = default) {
    return middleware.Invoke(input, next, token);
  }

}