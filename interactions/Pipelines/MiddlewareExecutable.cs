using Interactions.Core;

namespace Interactions.Pipelines;

internal sealed class MiddlewareExecutable<T1, T2, T3, T4>(Middleware<T1, T2, T3, T4> middleware, IExecutable<T2, T3> next) : IExecutable<T1, T4> {

  public T4 Execute(T1 input) {
    return middleware.Invoke(input, next);
  }

}