namespace Executables.Core.Executables;

internal sealed class AsyncFlattenExecutable<T1, T2>(IAsyncExecutable<T1, IAsyncExecutable<T1, T2>> inner) : IAsyncExecutable<T1, T2>, IAsyncExecutor<T1, T2> {

  private readonly IAsyncExecutor<T1, IAsyncExecutable<T1, T2>> _inner = inner.GetExecutor();

  IAsyncExecutor<T1, T2> IAsyncExecutable<T1, T2>.GetExecutor() {
    return this;
  }

  async ValueTask<T2> IAsyncExecutor<T1, T2>.Execute(T1 input, CancellationToken token) {
    return await (await _inner.Execute(input, token)).GetExecutor().Execute(input, token);
  }

}