using Executables.Operations;

namespace Executables.Core.Operators;

internal sealed class SuppressExceptionAsyncOperator<T1, T2, TEx> : AsyncExecutionOperator<T1, T1, T2, Optional<T2>> where TEx : Exception {

  public override async ValueTask<Optional<T2>> Invoke(T1 input, IAsyncExecutor<T1, T2> executor, CancellationToken token = default) {
    try {
      return new Optional<T2>(await executor.Execute(input, token));
    }
    catch (TEx) {
      return default;
    }
  }

}

internal sealed class SuppressExceptionAsyncOptionalOperator<T1, T2, TEx> : AsyncBehaviorOperator<T1, Optional<T2>> where TEx : Exception {

  public override async ValueTask<Optional<T2>> Invoke(T1 input, IAsyncExecutor<T1, Optional<T2>> executor, CancellationToken token = default) {
    try {
      return await executor.Execute(input, token);
    }
    catch (TEx) {
      return default;
    }
  }

}