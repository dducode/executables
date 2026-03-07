using Interactions.Core;
using Interactions.Core.Executables;

namespace Interactions.Policies;

internal sealed class TimeoutPolicy<T1, T2>(TimeSpan timeout) : AsyncPolicy<T1, T2> {

  public override async ValueTask<T2> Invoke(T1 input, IAsyncExecutable<T1, T2> executable, CancellationToken token = default) {
    using var cts = new CancellationTokenSource(timeout);
    using var linked = CancellationTokenSource.CreateLinkedTokenSource(token, cts.Token);

    try {
      return await executable.Execute(input, linked.Token);
    }
    catch (OperationCanceledException e) when (cts.IsCancellationRequested && !token.IsCancellationRequested) {
      throw new TimeoutException($"Invocation timed out after {timeout}", e);
    }
  }

}