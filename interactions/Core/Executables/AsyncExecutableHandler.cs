using Interactions.Handling;

namespace Interactions.Core.Executables;

internal sealed class AsyncExecutableHandler<T1, T2>(IAsyncExecutable<T1, T2> executable) : AsyncHandler<T1, T2> {

  private readonly IAsyncExecutor<T1, T2> _executor = executable.GetExecutor();

  protected override ValueTask<T2> HandleCore(T1 input, CancellationToken token = default) {
    return _executor.Execute(input, token);
  }

  protected override void DisposeCore() {
    (executable as IDisposable)?.Dispose();
  }

}