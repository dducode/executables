using Interactions.Core;

namespace Interactions.Executables;

internal sealed class RaceExecutable<T1, T2>(IAsyncExecutable<T1, T2> first, IAsyncExecutable<T1, T2> second)
  : IAsyncExecutable<T1, T2>, IAsyncExecutor<T1, T2> {

  private readonly IAsyncExecutor<T1, T2> _first = first.GetExecutor();
  private readonly IAsyncExecutor<T1, T2> _second = second.GetExecutor();

  IAsyncExecutor<T1, T2> IAsyncExecutable<T1, T2>.GetExecutor() {
    return this;
  }

  async ValueTask<T2> IAsyncExecutor<T1, T2>.Execute(T1 input, CancellationToken token) {
    ValueTask<T2> t1 = _first.Execute(input, token);
    if (t1.IsCompletedSuccessfully)
      return t1.Result;

    ValueTask<T2> t2 = _second.Execute(input, token);
    if (t2.IsCompletedSuccessfully)
      return t2.Result;

    Task<T2> task1 = t1.AsTask();
    Task<T2> task2 = t2.AsTask();

    return (await Task.WhenAny(task1, task2)).Result;
  }

}