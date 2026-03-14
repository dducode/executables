using Interactions.Core;

namespace Interactions;

public abstract class ExecutionOperator<T1, T2, T3, T4> {

  public abstract T4 Invoke(T1 input, IExecutor<T2, T3> executor);

}