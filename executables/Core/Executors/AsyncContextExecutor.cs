using Executables.Context;

namespace Executables.Core.Executors;

internal sealed class AsyncContextExecutor<T1, T2>(IAsyncExecutor<T1, T2> executor, ContextInit init) : IAsyncExecutor<T1, T2> {

  async ValueTask<T2> IAsyncExecutor<T1, T2>.Execute(T1 input, CancellationToken token) {
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