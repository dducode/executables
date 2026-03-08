namespace Interactions.Core.Executables;

internal sealed class IdentityExecutable<T> : IExecutable<T, T> {

  internal static IdentityExecutable<T> Instance { get; } = new();

  private IdentityExecutable() { }

  public T Execute(T input) {
    return input;
  }

}