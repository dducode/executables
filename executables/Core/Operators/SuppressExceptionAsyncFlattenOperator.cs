using Executables.Operations;

namespace Executables.Core.Operators;

internal sealed class SuppressExceptionAsyncFlattenOperator<T1, T2, TEx> : AsyncBehaviorOperator<T1, Optional<T2>> where TEx : Exception {

  internal static SuppressExceptionAsyncFlattenOperator<T1, T2, TEx> Instance { get; } = new();

  private SuppressExceptionAsyncFlattenOperator() { }

  public override async ValueTask<Optional<T2>> Invoke(T1 input, IAsyncExecutor<T1, Optional<T2>> executor, CancellationToken token = default) {
    try {
      return await executor.Execute(input, token);
    }
    catch (TEx) {
      return Optional<T2>.None;
    }
  }

}