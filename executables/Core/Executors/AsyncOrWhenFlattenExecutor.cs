namespace Executables.Core.Executors;

internal sealed class AsyncOrWhenFlattenExecutor<T1, T2>(
  IAsyncExecutor<T1, Optional<T2>> executor,
  Func<T1, bool> condition,
  IAsyncExecutor<T1, Optional<T2>> other)
  : IAsyncExecutor<T1, Optional<T2>> {

  async ValueTask<Optional<T2>> IAsyncExecutor<T1, Optional<T2>>.Execute(T1 input, CancellationToken token) {
    Optional<T2> result = await executor.Execute(input, token);
    return result.HasValue ? result : condition(input) ? await other.Execute(input, token) : Optional<T2>.None;
  }

}