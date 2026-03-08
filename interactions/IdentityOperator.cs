using Interactions.Core;

namespace Interactions;

internal sealed class IdentityOperator<T1, T2> : ExecutionOperator<T1, T1, T2, T2> {

  internal static IdentityOperator<T1, T2> Instance { get; } = new();

  private IdentityOperator() { }

  public override T2 Invoke(T1 input, IExecutable<T1, T2> next) {
    return next.Execute(input);
  }

}