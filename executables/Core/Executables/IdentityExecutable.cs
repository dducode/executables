namespace Executables.Core.Executables;

internal sealed class IdentityExecutable<T> : IExecutable<T, T>, IExecutor<T, T> {

  internal static IdentityExecutable<T> Instance { get; } = new();

  private IdentityExecutable() { }

  IExecutor<T, T> IExecutable<T, T>.GetExecutor() {
    return this;
  }

  T IExecutor<T, T>.Execute(T input) {
    return input;
  }

}