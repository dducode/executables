namespace Interactions.Core.Executables;

internal sealed class AnonymousExecutable<T1, T2>(Func<T1, T2> func) : IExecutable<T1, T2> {

  public T2 Execute(T1 input) {
    return func(input);
  }

}