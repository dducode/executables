using Interactions.Core;

namespace Interactions.Executables;

internal sealed class AsyncForkExecutable<T1, T2, T3>(IAsyncExecutable<T1, T2> first, IAsyncExecutable<T1, T3> second)
  : IAsyncExecutable<T1, (T2, T3)>, IAsyncExecutor<T1, (T2, T3)> {

  private readonly IAsyncExecutor<T1, T2> _first = first.GetExecutor();
  private readonly IAsyncExecutor<T1, T3> _second = second.GetExecutor();

  IAsyncExecutor<T1, (T2, T3)> IAsyncExecutable<T1, (T2, T3)>.GetExecutor() {
    return this;
  }

  ValueTask<(T2, T3)> IAsyncExecutor<T1, (T2, T3)>.Execute(T1 input, CancellationToken token) {
    token.ThrowIfCancellationRequested();

    ValueTask<T2> t1 = _first.Execute(input, token);
    ValueTask<T3> t2 = _second.Execute(input, token);

    if (t1.IsCompletedSuccessfully && t2.IsCompletedSuccessfully)
      return new ValueTask<(T2, T3)>((t1.Result, t2.Result));

    return Await(t1, t2);
  }

  private static async ValueTask<(T2, T3)> Await(ValueTask<T2> t1, ValueTask<T3> t2) {
    if (!t1.IsCompleted && !t2.IsCompleted) {
      Task<T2> task1 = t1.AsTask();
      Task<T3> task2 = t2.AsTask();
      await Task.WhenAll(task1, task2);
      return (task1.Result, task2.Result);
    }

    return (await t1, await t2);
  }

}