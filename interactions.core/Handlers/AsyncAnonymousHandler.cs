namespace Interactions.Core.Handlers;

internal sealed class AsyncAnonymousHandler<T1, T2>(AsyncFunc<T1, T2> func) : AsyncHandler<T1, T2> {

  protected override ValueTask<T2> ExecuteCore(T1 input, CancellationToken token = default) {
    return func(input, token);
  }

}