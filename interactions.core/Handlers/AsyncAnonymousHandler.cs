namespace Interactions.Core.Handlers;

internal sealed class AsyncAnonymousHandler<T1, T2>(AsyncFunc<T1, T2> func) : AsyncHandler<T1, T2> {

  public override ValueTask<T2> Handle(T1 input, CancellationToken token = default) {
    ThrowIfDisposed(nameof(AsyncAnonymousHandler<T1, T2>));
    return func(input, token);
  }

}