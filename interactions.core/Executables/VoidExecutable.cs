namespace Interactions.Core.Executables;

internal sealed class VoidExecutable<T>(IExecutable<T, Unit> inner) : IExecutable<T> {

  public void Execute(T input) {
    inner.Execute(input);
  }

}