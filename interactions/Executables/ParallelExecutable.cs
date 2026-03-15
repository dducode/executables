using Interactions.Core;

namespace Interactions.Executables;

internal sealed class ParallelExecutable<T1, T2, T3, T4>(IAsyncExecutable<T1, T3> first, IAsyncExecutable<T2, T4> second)
  : IAsyncExecutable<(T1, T2), (T3, T4)>, IAsyncExecutor<(T1, T2), (T3, T4)> {

  private readonly IAsyncExecutor<T1, T3> _first = first.GetExecutor();
  private readonly IAsyncExecutor<T2, T4> _second = second.GetExecutor();

  IAsyncExecutor<(T1, T2), (T3, T4)> IAsyncExecutable<(T1, T2), (T3, T4)>.GetExecutor() {
    return this;
  }

  async ValueTask<(T3, T4)> IAsyncExecutor<(T1, T2), (T3, T4)>.Execute((T1, T2) input, CancellationToken token) {
    ValueTask<T3> t1 = _first.Execute(input.Item1, token);
    ValueTask<T4> t2 = _second.Execute(input.Item2, token);

    if (!t1.IsCompleted && !t2.IsCompleted) {
      Task<T3> task1 = t1.AsTask();
      Task<T4> task2 = t2.AsTask();
      await Task.WhenAll(task1, task2);
      return (task1.Result, task2.Result);
    }

    if (!t1.IsCompleted)
      await t1;
    else if (!t2.IsCompleted)
      await t2;

    return (t1.Result, t2.Result);
  }

}