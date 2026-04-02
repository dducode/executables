using Executables.Operations;

namespace Executables.Core.Operators;

internal sealed class Map<T1, T2, T3, T4>(IExecutable<T1, T2> incoming, IExecutable<T3, T4> outgoing) : ExecutionOperator<T1, T2, T3, T4> {

  private readonly IExecutor<T1, T2> _incoming = incoming.GetExecutor();
  private readonly IExecutor<T3, T4> _outgoing = outgoing.GetExecutor();

  public override T4 Invoke(T1 input, IExecutor<T2, T3> executor) {
    return _outgoing.Execute(executor.Execute(_incoming.Execute(input)));
  }

}