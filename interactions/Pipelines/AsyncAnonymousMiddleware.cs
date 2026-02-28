using Interactions.Core;

namespace Interactions.Pipelines;

internal sealed class AsyncAnonymousMiddleware<T1, T2, T3, T4>(AsyncFunc<T1, AsyncHandler<T2, T3>, T4> pipeline) : AsyncMiddleware<T1, T2, T3, T4> {

  public override ValueTask<T4> Invoke(T1 input, AsyncHandler<T2, T3> next, CancellationToken token = default) {
    return pipeline(input, next, token);
  }

}