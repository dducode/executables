namespace Interactions.Core.Executables;

internal sealed class ExecutableCommand<T>(IExecutable<T, bool> inner) : ICommand<T>, IExecutor<T, bool> {

  private readonly IExecutor<T, bool> _inner = inner.GetExecutor();

  public bool Execute(T input) {
    return _inner.Execute(input);
  }

  public IExecutor<T, bool> GetExecutor() {
    return this;
  }

}