namespace Interactions.Core.Executables;

internal sealed class AsyncSecondMapExecutable<T1, T2, TNew>(IAsyncExecutable<T2, TNew> map)
  : IAsyncExecutable<(T1, T2), (T1, TNew)>, IAsyncExecutor<(T1, T2), (T1, TNew)> {

  private readonly IAsyncExecutor<T2, TNew> _map = map.GetExecutor();

  IAsyncExecutor<(T1, T2), (T1, TNew)> IAsyncExecutable<(T1, T2), (T1, TNew)>.GetExecutor() {
    return this;
  }

  ValueTask<(T1, TNew)> IAsyncExecutor<(T1, T2), (T1, TNew)>.Execute((T1, T2) input, CancellationToken token) {
    ValueTask<TNew> task = _map.Execute(input.Item2, token);
    if (task.IsCompleted)
      return new ValueTask<(T1, TNew)>((input.Item1, task.Result));

    return Await(input.Item1, task);
  }

  private static async ValueTask<(T1, TNew)> Await(T1 first, ValueTask<TNew> task) {
    return (first, await task);
  }

}