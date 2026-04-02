using Executables.Operations;

namespace Executables.Core.Operators;

internal sealed class SuppressExceptionFlattenOperator<T1, T2, TEx> : BehaviorOperator<T1, Optional<T2>> where TEx : Exception {

  internal static SuppressExceptionFlattenOperator<T1, T2, TEx> Instance { get; } = new();

  private SuppressExceptionFlattenOperator() { }

  public override Optional<T2> Invoke(T1 input, IExecutor<T1, Optional<T2>> executor) {
    try {
      return executor.Execute(input);
    }
    catch (TEx) {
      return Optional<T2>.None;
    }
  }

}