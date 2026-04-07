namespace Executables.Core.Iso;

internal sealed class InvertedIso<T1, T2>(IIso<T1, T2> iso) : IIso<T2, T1> {

  T1 IIso<T2, T1>.Forward(T2 input) {
    return iso.Backward(input);
  }

  T2 IIso<T2, T1>.Backward(T1 input) {
    return iso.Forward(input);
  }

}