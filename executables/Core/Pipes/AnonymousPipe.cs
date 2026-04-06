namespace Executables.Core.Pipes;

internal sealed class AnonymousPipe<T1, T2>(Func<T1, T2> pipe) : IPipe<T1, T2> {

  public T2 Apply(T1 input) {
    return pipe(input);
  }

}