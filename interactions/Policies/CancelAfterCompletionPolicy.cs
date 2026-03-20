using Interactions.Core;

namespace Interactions.Policies;

internal sealed class CancelAfterCompletionPolicy<T1, T2> : AsyncPolicy<T1, T2> {

  public override async ValueTask<T2> Invoke(T1 input, IAsyncExecutor<T1, T2> executor, CancellationToken token = default) {
    token.ThrowIfCancellationRequested();
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
    try {
      return await executor.Execute(input, cts.Token);
    }
    finally {
      cts.Cancel();
    }
  }

}