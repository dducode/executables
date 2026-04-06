namespace Executables.Core.Pipes;

internal sealed class CompositePipe<T1, T2, T3>(IPipe<T2, T3> first, IPipe<T1, T2> second) : IPipe<T1, T3> {

  T3 IPipe<T1, T3>.Apply(T1 input) {
    return first.Apply(second.Apply(input));
  }

}