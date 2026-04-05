namespace Executables.Core.Executors;

internal sealed class AsyncWhenFlattenExecutor<T1, T2>(Func<T1, bool> condition, IAsyncExecutor<T1, Optional<T2>> executor) : IAsyncExecutor<T1, Optional<T2>> {

  ValueTask<Optional<T2>> IAsyncExecutor<T1, Optional<T2>>.Execute(T1 input, CancellationToken token) {
    return condition(input) ? Await(input, executor, token) : new ValueTask<Optional<T2>>(Optional<T2>.None);
  }

  private static async ValueTask<Optional<T2>> Await(T1 input, IAsyncExecutor<T1, Optional<T2>> executor, CancellationToken token) {
    return await executor.Execute(input, token);
  }

}