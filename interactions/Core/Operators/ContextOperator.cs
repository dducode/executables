using Interactions.Context;
using Interactions.Operations;

namespace Interactions.Core.Operators;

internal sealed class ContextOperator<T1, T2>(ContextInit init) : BehaviorOperator<T1, T2> {

  public override T2 Invoke(T1 input, IExecutor<T1, T2> executor) {
    IReadonlyContext previous = InteractionContext.Current;
    using var current = new InteractionContext(previous);
    init(new ContextWriter(current));
    InteractionContext.Current = current;

    try {
      return executor.Execute(input);
    }
    finally {
      InteractionContext.Current = previous;
    }
  }

}