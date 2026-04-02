namespace Executables.Core.Executables;

internal sealed class AsyncWhenFlattenExecutable<T1, T2>(Func<T1, bool> condition, IAsyncExecutable<T1, Optional<T2>> inner)
  : IAsyncExecutable<T1, Optional<T2>>, IAsyncExecutor<T1, Optional<T2>> {

  private readonly IAsyncExecutor<T1, Optional<T2>> _inner = inner.GetExecutor();

  IAsyncExecutor<T1, Optional<T2>> IAsyncExecutable<T1, Optional<T2>>.GetExecutor() {
    return this;
  }

  ValueTask<Optional<T2>> IAsyncExecutor<T1, Optional<T2>>.Execute(T1 input, CancellationToken token) {
    return condition(input) ? Await(input, _inner, token) : new ValueTask<Optional<T2>>(Optional<T2>.None);
  }

  private static async ValueTask<Optional<T2>> Await(T1 input, IAsyncExecutor<T1, Optional<T2>> executor, CancellationToken token) {
    return await executor.Execute(input, token);
  }

}