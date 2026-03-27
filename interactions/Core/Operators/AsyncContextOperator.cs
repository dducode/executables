using Interactions.Context;
using Interactions.Operations;

namespace Interactions.Core.Operators;

internal sealed class AsyncContextOperator<T1, T2>(ContextInit init) : AsyncBehaviorOperator<T1, T2> {

  public override async ValueTask<T2> Invoke(T1 input, IAsyncExecutor<T1, T2> executor, CancellationToken token = default) {
    IReadonlyContext previous = InteractionContext.Current;
    using var current = new InteractionContext(previous);
    init(new ContextWriter(current));
    InteractionContext.Current = current;

    try {
      return await executor.Execute(input, token);
    }
    finally {
      InteractionContext.Current = previous;
    }
  }

}