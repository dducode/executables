using Interactions.Core;

namespace Interactions.Executables;

internal sealed class AsyncSecondMapExecutable<T1, T2, TNew>(IAsyncExecutable<T2, TNew> map)
  : IAsyncExecutable<(T1, T2), (T1, TNew)>, IAsyncExecutor<(T1, T2), (T1, TNew)> {

  private readonly IAsyncExecutor<T2, TNew> _map = map.GetExecutor();

  IAsyncExecutor<(T1, T2), (T1, TNew)> IAsyncExecutable<(T1, T2), (T1, TNew)>.GetExecutor() {
    return this;
  }

  async ValueTask<(T1, TNew)> IAsyncExecutor<(T1, T2), (T1, TNew)>.Execute((T1, T2) input, CancellationToken token) {
    return (input.Item1, await _map.Execute(input.Item2, token));
  }

}