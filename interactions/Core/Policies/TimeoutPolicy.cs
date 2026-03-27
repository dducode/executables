using Interactions.Policies;

namespace Interactions.Core.Policies;

internal sealed class TimeoutPolicy<T1, T2>(TimeSpan timeout) : AsyncPolicy<T1, T2> {

  public override async ValueTask<T2> Invoke(T1 input, IAsyncExecutor<T1, T2> executor, CancellationToken token = default) {
    using var cts = new CancellationTokenSource(timeout);
    using var linked = CancellationTokenSource.CreateLinkedTokenSource(token, cts.Token);

    try {
      return await executor.Execute(input, linked.Token);
    }
    catch (OperationCanceledException e) when (cts.IsCancellationRequested && !token.IsCancellationRequested) {
      throw new TimeoutException($"Invocation timed out after {timeout}", e);
    }
  }

}