using Executables.Handling;

namespace Executables.Core.Executables;

internal sealed class ExecutableHandler<T1, T2>(IExecutable<T1, T2> executable) : Handler<T1, T2> {

  private readonly IExecutor<T1, T2> _executor = executable.GetExecutor();

  protected override T2 HandleCore(T1 input) {
    return _executor.Execute(input);
  }

  protected override void DisposeCore() {
    (executable as IDisposable)?.Dispose();
  }

}