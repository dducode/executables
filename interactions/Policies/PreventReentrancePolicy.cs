using Interactions.Core;

namespace Interactions.Policies;

internal sealed class PreventReentrancePolicy<T1, T2> : Policy<T1, T2> {

  private readonly ThreadLocal<int> _enterCount = new();

  public override T2 Invoke(T1 input, IExecutor<T1, T2> executor) {
    try {
      return ++_enterCount.Value <= 1 ? executor.Execute(input) : throw new ReentranceException("Nested re-entry is not allowed");
    }
    finally {
      _enterCount.Value--;
    }
  }

}