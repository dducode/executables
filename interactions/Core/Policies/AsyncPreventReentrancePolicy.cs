using Interactions.Policies;

namespace Interactions.Core.Policies;

internal sealed class AsyncPreventReentrancePolicy<T1, T2> : AsyncPolicy<T1, T2> {

  private readonly AsyncLocal<int> _enterCount = new();

  public override async ValueTask<T2> Invoke(T1 input, IAsyncExecutor<T1, T2> executor, CancellationToken token = default) {
    token.ThrowIfCancellationRequested();

    try {
      return ++_enterCount.Value <= 1 ? await executor.Execute(input, token) : throw new ReentranceException("Nested re-entry is not allowed");
    }
    finally {
      _enterCount.Value--;
    }
  }

}