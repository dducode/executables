using Interactions.Core;

namespace Interactions.Policies;

internal sealed class IdentityPolicy<T1, T2> : Policy<T1, T2> {

  internal static IdentityPolicy<T1, T2> Instance { get; } = new();

  private IdentityPolicy() { }

  public override T2 Invoke(T1 input, IExecutable<T1, T2> executable) {
    return executable.Execute(input);
  }

}