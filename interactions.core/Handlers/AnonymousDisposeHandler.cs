namespace Interactions.Core.Handlers;

internal sealed class AnonymousDisposeHandler<T1, T2>(Handler<T1, T2> inner, Action dispose) : Handler<T1, T2> {

  protected override T2 HandleCore(T1 input) {
    return inner.Handle(input);
  }

  protected override void DisposeCore() {
    try {
      dispose();
    }
    finally {
      inner.Dispose();
    }
  }

}