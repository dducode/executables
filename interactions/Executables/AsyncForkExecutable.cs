using Interactions.Core;

namespace Interactions.Executables;

internal sealed class AsyncForkExecutable<T1, T2, T3>(IAsyncExecutable<T1, T2> first, IAsyncExecutable<T1, T3> second)
  : IAsyncExecutable<T1, (T2, T3)>, IAsyncExecutor<T1, (T2, T3)> {

  private readonly IAsyncExecutor<T1, T2> _first = first.GetExecutor();
  private readonly IAsyncExecutor<T1, T3> _second = second.GetExecutor();

  IAsyncExecutor<T1, (T2, T3)> IAsyncExecutable<T1, (T2, T3)>.GetExecutor() {
    return this;
  }

  async ValueTask<(T2, T3)> IAsyncExecutor<T1, (T2, T3)>.Execute(T1 input, CancellationToken token) {
    ValueTask<T2> t1 = _first.Execute(input, token);
    ValueTask<T3> t2 = _second.Execute(input, token);

    if (!t1.IsCompleted && !t2.IsCompleted)
      await Task.WhenAll(t1.AsTask(), t2.AsTask());
    else if (!t1.IsCompleted)
      await t1;
    else if (!t2.IsCompleted)
      await t2;

    return (t1.Result, t2.Result);
  }

}