using Executables.Handling;

namespace Executables.Core.Handlers;

internal sealed class AsyncAnonymousDisposeHandler<T1, T2> : AsyncHandler<T1, T2> {

  private readonly AsyncHandler<T1, T2> _inner;
  private readonly Action _dispose;

  public AsyncAnonymousDisposeHandler(AsyncHandler<T1, T2> inner, Action dispose) {
    _inner = inner;
    _dispose = dispose;
    RegisterHandle(_inner);
    _inner.RegisterHandle(this);
  }

  protected override ValueTask<T2> HandleCore(T1 input, CancellationToken token = default) {
    return _inner.Handle(input, token);
  }

  protected override void DisposeCore() {
    _dispose();
  }

}