namespace Executables.Core.Executables;

internal sealed class AsyncFirstMapExecutable<T1, T2, TNew>(IAsyncExecutable<T1, TNew> map)
  : IAsyncExecutable<(T1, T2), (TNew, T2)>, IAsyncExecutor<(T1, T2), (TNew, T2)> {

  private readonly IAsyncExecutor<T1, TNew> _map = map.GetExecutor();

  IAsyncExecutor<(T1, T2), (TNew, T2)> IAsyncExecutable<(T1, T2), (TNew, T2)>.GetExecutor() {
    return this;
  }

  ValueTask<(TNew, T2)> IAsyncExecutor<(T1, T2), (TNew, T2)>.Execute((T1, T2) input, CancellationToken token) {
    ValueTask<TNew> task = _map.Execute(input.Item1, token);
    if (task.IsCompleted)
      return new ValueTask<(TNew, T2)>((task.Result, input.Item2));

    return Await(task, input.Item2);
  }

  private static async ValueTask<(TNew, T2)> Await(ValueTask<TNew> task, T2 second) {
    return (await task, second);
  }

}