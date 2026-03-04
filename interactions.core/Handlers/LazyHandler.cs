namespace Interactions.Core.Handlers;

internal sealed class LazyHandler<T1, T2>(IResolver<Handler<T1, T2>> resolver) : Handler<T1, T2> {

  private Handler<T1, T2> _inner;
  private readonly object _lock = new();

  protected override T2 HandleCore(T1 input) {
    Handler<T1, T2> inner = Volatile.Read(ref _inner);

    if (inner == null) {
      lock (_lock) {
        inner = _inner;
        if (inner == null)
          _inner = inner = resolver.Resolve() ?? throw new InvalidOperationException($"The handler could not be resolved by {resolver.GetType().Name}");
      }
    }

    return inner.Handle(input);
  }

  protected override void DisposeCore() {
    try {
      _inner?.Dispose();
    }
    finally {
      _inner = null;
    }
  }

}