using Executables.Context;
using Executables.Operations;

namespace Executables.Core.Operators;

internal sealed class ContextOperator<T1, T2>(ContextInit init) : BehaviorOperator<T1, T2> {

  public override T2 Invoke(T1 input, IExecutor<T1, T2> executor) {
    IReadonlyContext previous = ExecutableContext.Current;
    using var current = new ExecutableContext(previous);
    init(new ContextWriter(current));
    ExecutableContext.Current = current;

    try {
      return executor.Execute(input);
    }
    finally {
      ExecutableContext.Current = previous;
    }
  }

}