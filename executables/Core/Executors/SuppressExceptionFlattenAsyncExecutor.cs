namespace Executables.Core.Executors;

internal sealed class SuppressExceptionFlattenAsyncExecutor<T1, T2, TEx>(IAsyncExecutor<T1, Optional<T2>> executor)
  : IAsyncExecutor<T1, Optional<T2>> where TEx : Exception {

  async ValueTask<Optional<T2>> IAsyncExecutor<T1, Optional<T2>>.Execute(T1 input, CancellationToken token) {
    try {
      return await executor.Execute(input, token);
    }
    catch (TEx) {
      return Optional<T2>.None;
    }
  }

}