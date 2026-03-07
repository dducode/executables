namespace Interactions.Core.Handlers;

internal sealed class AnonymousDisposeHandler<T1, T2>(Handler<T1, T2> inner, Action dispose) : Handler<T1, T2> {

  protected override T2 ExecuteCore(T1 input) {
    return inner.Execute(input);
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