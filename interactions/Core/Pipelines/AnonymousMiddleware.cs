using Interactions.Pipelines;

namespace Interactions.Core.Pipelines;

internal sealed class AnonymousMiddleware<T1, T2, T3, T4>(Func<T1, IExecutor<T2, T3>, T4> pipeline) : Middleware<T1, T2, T3, T4> {

  public override T4 Invoke(T1 input, IExecutor<T2, T3> executor) {
    return pipeline(input, executor);
  }

}