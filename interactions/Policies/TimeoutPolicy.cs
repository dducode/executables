using Interactions.Core;

namespace Interactions.Policies;

internal sealed class TimeoutPolicy<T1, T2>(TimeSpan timeout) : AsyncPolicy<T1, T2> {

  public override async ValueTask<T2> Execute(T1 input, AsyncFunc<T1, T2> invocation, CancellationToken token) {
    using var cts = new CancellationTokenSource(timeout);
    using var linked = CancellationTokenSource.CreateLinkedTokenSource(token, cts.Token);

    try {
      return await invocation.Invoke(input, linked.Token);
    }
    catch (OperationCanceledException e) when (cts.IsCancellationRequested && !token.IsCancellationRequested) {
      throw new TimeoutException($"Invocation timed out after {timeout}", e);
    }
  }

}