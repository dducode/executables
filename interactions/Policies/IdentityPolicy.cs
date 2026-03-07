using Interactions.Core;
using Interactions.Core.Executables;

namespace Interactions.Policies;

internal sealed class IdentityPolicy<T1, T2> : Policy<T1, T2> {

  internal static IdentityPolicy<T1, T2> Instance { get; } = new();

  private IdentityPolicy() { }

  public override T2 Invoke(T1 input, IExecutable<T1, T2> executable) {
    return executable.Execute(input);
  }

}

internal sealed class AsyncIdentityPolicy<T1, T2> : AsyncPolicy<T1, T2> {

  internal static AsyncIdentityPolicy<T1, T2> Instance { get; } = new();

  private AsyncIdentityPolicy() { }

  public override ValueTask<T2> Invoke(T1 input, IAsyncExecutable<T1, T2> executable, CancellationToken token = default) {
    return executable.Execute(input, token);
  }

}