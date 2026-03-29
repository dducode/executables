namespace Executables.Core.Commands;

internal sealed class CompositeCommand<T>(ICommand<T> first, ICommand<T> second) : ICommand<T>, IExecutor<T, bool> {

  public bool Execute(T input) {
    return first.Execute(input) && second.Execute(input);
  }

  IExecutor<T, bool> IExecutable<T, bool>.GetExecutor() {
    return this;
  }

}