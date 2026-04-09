using Executables.Context;

namespace Executables.Core.Executors;

internal sealed class ContextExecutor<T1, T2>(IExecutor<T1, T2> executor, ContextInit init) : IExecutor<T1, T2> {

  T2 IExecutor<T1, T2>.Execute(T1 input) {
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