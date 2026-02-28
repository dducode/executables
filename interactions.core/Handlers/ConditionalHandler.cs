using Interactions.Core.Extensions;

namespace Interactions.Core.Handlers;

internal sealed class ConditionalHandler<T1, T2>(
  Func<bool> condition,
  Handler<T1, T2> mainHandler,
  Handler<T1, T2> otherHandler) : Handler<T1, T2> {

  public override T2 Handle(T1 input) {
    ThrowIfDisposed(nameof(ConditionalHandler<T1, T2>));
    return condition() ? mainHandler.Handle(input) : otherHandler.Handle(input);
  }

  protected override void DisposeCore() {
    mainHandler.Compose(otherHandler).Dispose();
  }

}

internal sealed class AsyncConditionalHandler<T1, T2>(
  Func<bool> condition,
  AsyncHandler<T1, T2> mainHandler,
  AsyncHandler<T1, T2> otherHandler) : AsyncHandler<T1, T2> {

  public override ValueTask<T2> Handle(T1 input, CancellationToken token = default) {
    ThrowIfDisposed(nameof(AsyncConditionalHandler<T1, T2>));
    return condition() ? mainHandler.Handle(input, token) : otherHandler.Handle(input, token);
  }

  protected override void DisposeCore() {
    mainHandler.Compose(otherHandler).Dispose();
  }

}