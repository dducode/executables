namespace Executables.Core.Iso;

internal sealed class IdentityIso<T> : IIso<T, T> {

  internal static IdentityIso<T> Instance { get; } = new();

  private IdentityIso() { }

  T IIso<T, T>.Forward(T input) {
    return input;
  }

  T IIso<T, T>.Backward(T input) {
    return input;
  }

}