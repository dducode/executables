using Executables.Context;
using Executables.Operations;

namespace Executables.Core.Operators;

internal sealed class AsyncContextOperator<T1, T2>(ContextInit init) : AsyncBehaviorOperator<T1, T2> {

  public override async ValueTask<T2> Invoke(T1 input, IAsyncExecutor<T1, T2> executor, CancellationToken token = default) {
    IReadonlyContext previous = ExecutableContext.Current;
    using var current = new ExecutableContext(previous);
    init(new ContextWriter(current));
    ExecutableContext.Current = current;

    try {
      return await executor.Execute(input, token);
    }
    finally {
      ExecutableContext.Current = previous;
    }
  }

}