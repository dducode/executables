using Interactions.Operations;

namespace Interactions.Core.Operators;

internal sealed class AnonymousOperator<T1, T2, T3, T4>(Func<T1, IExecutor<T2, T3>, T4> operation) : ExecutionOperator<T1, T2, T3, T4> {

  public override T4 Invoke(T1 input, IExecutor<T2, T3> executor) {
    return operation(input, executor);
  }

}