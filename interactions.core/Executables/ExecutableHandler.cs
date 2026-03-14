namespace Interactions.Core.Executables;

internal sealed class ExecutableHandler<T1, T2>(IExecutable<T1, T2> inner) : Handler<T1, T2> {

  private readonly IExecutor<T1, T2> _inner = inner.GetExecutor();

  protected override T2 HandleCore(T1 input) {
    return _inner.Execute(input);
  }

  protected override void DisposeCore() {
    (_inner as IDisposable)?.Dispose();
  }

}