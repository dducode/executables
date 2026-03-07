namespace Interactions.Core.Executables;

internal sealed class AsyncExecutableHandlerWrapper<T1, T2>(IAsyncExecutable<T1, T2> inner) : AsyncHandler<T1, T2> {

  protected override ValueTask<T2> ExecuteCore(T1 input, CancellationToken token = default) {
    return inner.Execute(input, token);
  }

}