using Interactions.Core;

namespace Interactions.Executables;

internal sealed class AsyncCompositeExecutable<T1, T2, T3>(IAsyncExecutable<T1, T2> first, IAsyncExecutable<T2, T3> second)
  : IAsyncExecutable<T1, T3>, IAsyncExecutor<T1, T3> {

  private readonly IAsyncExecutor<T1, T2> _first = first.GetExecutor();
  private readonly IAsyncExecutor<T2, T3> _second = second.GetExecutor();

  IAsyncExecutor<T1, T3> IAsyncExecutable<T1, T3>.GetExecutor() {
    return this;
  }

  async ValueTask<T3> IAsyncExecutor<T1, T3>.Execute(T1 input, CancellationToken token) {
    return await _second.Execute(await _first.Execute(input, token), token);
  }

}