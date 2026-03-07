namespace Interactions.Core.Executables;

internal sealed class IdentityExecutable<T> : IExecutable<T, T> {

  internal static readonly IdentityExecutable<T> Instance = new();

  private IdentityExecutable() { }

  public T Execute(T input) {
    return input;
  }

}