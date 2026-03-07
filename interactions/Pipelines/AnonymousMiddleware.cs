using Interactions.Core;
using Interactions.Core.Executables;

namespace Interactions.Pipelines;

internal sealed class AnonymousMiddleware<T1, T2, T3, T4>(Func<T1, IExecutable<T2, T3>, T4> pipeline) : Middleware<T1, T2, T3, T4> {

  public override T4 Invoke(T1 input, IExecutable<T2, T3> executable) {
    return pipeline(input, executable);
  }

}