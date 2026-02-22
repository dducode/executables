using Interactions.Core;

namespace Interactions.Pipelines;

public abstract class Pipeline<T1, T2, T3, T4> {

  public abstract T4 Invoke(T1 input, Handler<T2, T3> next);

}