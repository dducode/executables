namespace Interactions.Core.Executables;

internal sealed class ExecutableHandlerWrapper<T1, T2>(IExecutable<T1, T2> inner) : Handler<T1, T2> {

  protected override T2 ExecuteCore(T1 input) {
    return inner.Execute(input);
  }

  protected override void DisposeCore() {
    (inner as IDisposable)?.Dispose();
  }

}

internal sealed class ExecutableHandlerWrapper<T>(IExecutable<T> inner) : Handler<T, Unit> {

  protected override Unit ExecuteCore(T input) {
    inner.Execute(input);
    return default;
  }

}