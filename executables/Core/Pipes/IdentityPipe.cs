namespace Executables.Core.Pipes;

internal sealed class IdentityPipe<T> : IPipe<T, T> {

  internal static IdentityPipe<T> Instance { get; } = new();

  private IdentityPipe() { }

  T IPipe<T, T>.Apply(T input) {
    return input;
  }

}