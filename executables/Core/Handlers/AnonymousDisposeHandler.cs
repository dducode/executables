using Executables.Handling;

namespace Executables.Core.Handlers;

internal sealed class AnonymousDisposeHandler<T1, T2> : Handler<T1, T2> {

  private readonly Handler<T1, T2> _inner;
  private readonly Action _dispose;

  public AnonymousDisposeHandler(Handler<T1, T2> inner, Action dispose) {
    _inner = inner;
    _dispose = dispose;
    RegisterHandle(_inner);
    _inner.RegisterHandle(this);
  }

  protected override T2 HandleCore(T1 input) {
    return _inner.Handle(input);
  }

  protected override void DisposeCore() {
    _dispose();
  }

}