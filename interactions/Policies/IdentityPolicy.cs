using Interactions.Core;

namespace Interactions.Policies;

internal sealed class IdentityPolicy<T1, T2> : Policy<T1, T2> {

  internal static IdentityPolicy<T1, T2> Instance { get; } = new();

  private IdentityPolicy() { }

  public override T2 Execute(T1 input, Func<T1, T2> invocation) {
    return invocation(input);
  }

}

internal sealed class AsyncIdentityPolicy<T1, T2> : AsyncPolicy<T1, T2> {

  internal static AsyncIdentityPolicy<T1, T2> Instance { get; } = new();

  private AsyncIdentityPolicy() { }

  public override ValueTask<T2> Execute(T1 input, AsyncFunc<T1, T2> invocation, CancellationToken token) {
    return invocation(input, token);
  }

}