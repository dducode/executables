using Executables.Operations;

namespace Executables.Core.Operators;

internal sealed class IdentityOperator<T1, T2> : ExecutionOperator<T1, T1, T2, T2> {

  internal static IdentityOperator<T1, T2> Instance { get; } = new();

  private IdentityOperator() { }

  public override T2 Invoke(T1 input, IExecutor<T1, T2> executor) {
    return executor.Execute(input);
  }

}