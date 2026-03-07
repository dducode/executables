using Interactions.Core;

namespace Interactions.Operations;

public abstract class ExecutionOperator<T1, T2, T3, T4> {

  public abstract T4 Invoke(T1 input, IExecutable<T2, T3> next);

}