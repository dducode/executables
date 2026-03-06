namespace Interactions.Core;

public abstract class ExecutionOperator<T1, T2, T3, T4> {

  public abstract T4 Invoke(T1 input, IExecutable<T2, T3> next);

}

public abstract class AsyncExecutionOperator<T1, T2, T3, T4> {

  public abstract ValueTask<T4> Invoke(T1 input, IAsyncExecutable<T2, T3> next, CancellationToken token = default);

}