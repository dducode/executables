using Executables.Operations;

namespace Executables.Core.Operators;

internal sealed class ResultOperator<T1, T2> : ExecutionOperator<T1, T1, T2, Result<T2>> {

  internal static ResultOperator<T1, T2> Instance { get; } = new();

  private ResultOperator() { }

  public override Result<T2> Invoke(T1 input, IExecutor<T1, T2> executor) {
    try {
      return Result<T2>.FromResult(executor.Execute(input));
    }
    catch (Exception e) {
      return Result<T2>.FromException(e);
    }
  }

}