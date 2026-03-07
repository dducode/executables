namespace Interactions.Core.Handlers;

internal sealed class ExecutableHandlerProxy<T1, T2>(IExecutable<T1, T2> inner) : Handler<T1, T2> {

  protected override T2 ExecuteCore(T1 input) {
    return inner.Execute(input);
  }

}

internal sealed class AsyncExecutableHandlerProxy<T1, T2>(IAsyncExecutable<T1, T2> inner) : AsyncHandler<T1, T2> {

  protected override ValueTask<T2> ExecuteCore(T1 input, CancellationToken token = default) {
    return inner.Execute(input, token);
  }

}