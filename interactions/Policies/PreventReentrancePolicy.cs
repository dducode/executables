using Interactions.Core.Executables;

namespace Interactions.Policies;

internal sealed class PreventReentrancePolicy<T1, T2> : Policy<T1, T2> {

  private readonly ThreadLocal<int> _enterCount = new();

  public override T2 Invoke(T1 input, IExecutable<T1, T2> executable) {
    try {
      return ++_enterCount.Value <= 1 ? executable.Execute(input) : throw new ReentranceException("Nested re-entry is not allowed");
    }
    finally {
      _enterCount.Value--;
    }
  }

}

internal sealed class AsyncPreventReentrancePolicy<T1, T2> : AsyncPolicy<T1, T2> {

  private readonly AsyncLocal<int> _enterCount = new();

  public override async ValueTask<T2> Invoke(T1 input, IAsyncExecutable<T1, T2> executable, CancellationToken token = default) {
    token.ThrowIfCancellationRequested();

    try {
      return ++_enterCount.Value <= 1 ? await executable.Execute(input, token) : throw new ReentranceException("Nested re-entry is not allowed");
    }
    finally {
      _enterCount.Value--;
    }
  }

}