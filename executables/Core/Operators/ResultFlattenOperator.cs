using Executables.Operations;

namespace Executables.Core.Operators;

internal sealed class ResultFlattenOperator<T1, T2> : BehaviorOperator<T1, Result<T2>> {

  internal static ResultFlattenOperator<T1, T2> Instance { get; } = new();

  private ResultFlattenOperator() { }

  public override Result<T2> Invoke(T1 input, IExecutor<T1, Result<T2>> executor) {
    try {
      return executor.Execute(input);
    }
    catch (Exception e) {
      return Result<T2>.FromException(e);
    }
  }

}