namespace Interactions.Operations;

internal sealed class AsyncMap<T1, T2, T3, T4>(IExecutable<T1, T2> incoming, IExecutable<T3, T4> outgoing)
  : AsyncExecutionOperator<T1, T2, T3, T4> {

  private readonly IExecutor<T1, T2> _incoming = incoming.GetExecutor();
  private readonly IExecutor<T3, T4> _outgoing = outgoing.GetExecutor();

  public override async ValueTask<T4> Invoke(T1 input, IAsyncExecutor<T2, T3> executor, CancellationToken token = default) {
    return _outgoing.Execute(await executor.Execute(_incoming.Execute(input), token));
  }

}