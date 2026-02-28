namespace Interactions.Core.Handlers;

internal sealed class AsyncLazyHandler<T1, T2>(IResolver<AsyncHandler<T1, T2>> resolver) : AsyncHandler<T1, T2> {

  private AsyncHandler<T1, T2> _inner;
  private readonly object _lock = new();

  public override ValueTask<T2> Handle(T1 input, CancellationToken token = default) {
    ThrowIfDisposed(nameof(AsyncLazyHandler<T1, T2>));
    AsyncHandler<T1, T2> inner = Volatile.Read(ref _inner);

    if (inner == null) {
      lock (_lock) {
        inner = _inner;
        if (inner == null)
          _inner = inner = resolver.Resolve() ?? throw new InvalidOperationException($"The handler could not be resolved by {resolver.GetType().Name}");
      }
    }

    return inner.Handle(input, token);
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