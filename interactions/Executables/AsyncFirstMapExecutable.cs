using Interactions.Core;

namespace Interactions.Executables;

internal sealed class AsyncFirstMapExecutable<T1, T2, TNew>(IAsyncExecutable<T1, TNew> map)
  : IAsyncExecutable<(T1, T2), (TNew, T2)>, IAsyncExecutor<(T1, T2), (TNew, T2)> {

  private readonly IAsyncExecutor<T1, TNew> _map = map.GetExecutor();

  IAsyncExecutor<(T1, T2), (TNew, T2)> IAsyncExecutable<(T1, T2), (TNew, T2)>.GetExecutor() {
    return this;
  }

  async ValueTask<(TNew, T2)> IAsyncExecutor<(T1, T2), (TNew, T2)>.Execute((T1, T2) input, CancellationToken token) {
    return (await _map.Execute(input.Item1, token), input.Item2);
  }

}