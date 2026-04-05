namespace Executables.Core.Executors;

internal sealed class ResultExecutor<T1, T2>(IExecutor<T1, T2> executor) : IExecutor<T1, Result<T2>> {

  Result<T2> IExecutor<T1, Result<T2>>.Execute(T1 input) {
    try {
      return Result<T2>.FromResult(executor.Execute(input));
    }
    catch (Exception e) {
      return Result<T2>.FromException(e);
    }
  }

}