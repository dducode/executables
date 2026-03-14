using Interactions.Core;

namespace Interactions;

public abstract class AsyncExecutionOperator<T1, T2, T3, T4> {

  public abstract ValueTask<T4> Invoke(T1 input, IAsyncExecutor<T2, T3> executor, CancellationToken token = default);

}