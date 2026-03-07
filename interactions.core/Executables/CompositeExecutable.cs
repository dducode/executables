namespace Interactions.Core.Executables;

internal sealed class CompositeExecutable<T1, T2, T3>(IExecutable<T1, T2> first, IExecutable<T2, T3> second) : IExecutable<T1, T3> {

  public T3 Execute(T1 input) {
    return second.Execute(first.Execute(input));
  }

}