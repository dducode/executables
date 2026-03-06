using Interactions.Core;

namespace Interactions.Policies;

internal sealed class PreventReentrancePolicy<T1, T2> : Policy<T1, T2> {

  private readonly ThreadLocal<int> _enterCount = new();

  public override T2 Execute(T1 input, Func<T1, T2> invocation) {
    try {
      return ++_enterCount.Value <= 1 ? invocation(input) : throw new ReentranceException("Nested re-entry is not allowed");
    }
    finally {
      _enterCount.Value--;
    }
  }

}

internal sealed class AsyncPreventReentrancePolicy<T1, T2> : AsyncPolicy<T1, T2> {

  private readonly AsyncLocal<int> _enterCount = new();

  public override async ValueTask<T2> Execute(T1 input, AsyncFunc<T1, T2> invocation, CancellationToken token) {
    token.ThrowIfCancellationRequested();

    try {
      return ++_enterCount.Value <= 1 ? await invocation(input, token) : throw new ReentranceException("Nested re-entry is not allowed");
    }
    finally {
      _enterCount.Value--;
    }
  }

}