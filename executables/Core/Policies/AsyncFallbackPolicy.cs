using Executables.Policies;

namespace Executables.Core.Policies;

internal sealed class AsyncFallbackPolicy<T1, T2, TEx>(Func<T1, TEx, T2> fallback) : AsyncPolicy<T1, T2> where TEx : Exception {

  public override async ValueTask<T2> Invoke(T1 input, IAsyncExecutor<T1, T2> executor, CancellationToken token = default) {
    token.ThrowIfCancellationRequested();

    try {
      return await executor.Execute(input, token);
    }
    catch (TEx e) {
      return fallback(input, e);
    }
  }

}