using Interactions.Core;

namespace Interactions.Operations;

internal sealed class SuppressExceptionOperator<T1, T2, TEx> : ExecutionOperator<T1, T1, T2, Optional<T2>> where TEx : Exception {

  public override Optional<T2> Invoke(T1 input, IExecutor<T1, T2> executor) {
    try {
      return new Optional<T2>(executor.Execute(input));
    }
    catch (TEx) {
      return default;
    }
  }

}

internal sealed class SuppressExceptionOptionalOperator<T1, T2, TEx> : BehaviorOperator<T1, Optional<T2>> where TEx : Exception {

  public override Optional<T2> Invoke(T1 input, IExecutor<T1, Optional<T2>> executor) {
    try {
      return executor.Execute(input);
    }
    catch (TEx) {
      return default;
    }
  }

}