using Executables.Operations;

namespace Executables.Core.Operators;

internal sealed class SuppressExceptionOperator<T1, T2, TEx> : ExecutionOperator<T1, T1, T2, Optional<T2>> where TEx : Exception {

  internal static SuppressExceptionOperator<T1, T2, TEx> Instance { get; } = new();

  private SuppressExceptionOperator() { }

  public override Optional<T2> Invoke(T1 input, IExecutor<T1, T2> executor) {
    try {
      return new Optional<T2>(executor.Execute(input));
    }
    catch (TEx) {
      return Optional<T2>.None;
    }
  }

}