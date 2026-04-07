namespace Executables.Core.Iso;

internal sealed class CompositeIso<T1, T2, T3>(IIso<T2, T3> first, IIso<T1, T2> second) : IIso<T1, T3> {

  T3 IIso<T1, T3>.Forward(T1 input) {
    return first.Forward(second.Forward(input));
  }

  T1 IIso<T1, T3>.Backward(T3 input) {
    return second.Backward(first.Backward(input));
  }

}