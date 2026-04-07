namespace Executables.Core.Iso;

internal sealed class AnonymousIso<T1, T2>(Func<T1, T2> forward, Func<T2, T1> backward) : IIso<T1, T2> {

  T2 IIso<T1, T2>.Forward(T1 input) {
    return forward(input);
  }

  T1 IIso<T1, T2>.Backward(T2 input) {
    return backward(input);
  }

}