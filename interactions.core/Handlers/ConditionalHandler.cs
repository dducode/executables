using Interactions.Core.Extensions;

namespace Interactions.Core.Handlers;

internal sealed class ConditionalHandler<T1, T2>(
  Func<bool> condition,
  Handler<T1, T2> mainHandler,
  Handler<T1, T2> otherHandler) : Handler<T1, T2> {

  protected override T2 ExecuteCore(T1 input) {
    return condition() ? mainHandler.Execute(input) : otherHandler.Execute(input);
  }

  protected override void DisposeCore() {
    mainHandler.Compose(otherHandler).Dispose();
  }

}

internal sealed class AsyncConditionalHandler<T1, T2>(
  Func<bool> condition,
  AsyncHandler<T1, T2> mainHandler,
  AsyncHandler<T1, T2> otherHandler) : AsyncHandler<T1, T2> {

  protected override ValueTask<T2> ExecuteCore(T1 input, CancellationToken token = default) {
    return condition() ? mainHandler.Execute(input, token) : otherHandler.Execute(input, token);
  }

  protected override void DisposeCore() {
    mainHandler.Compose(otherHandler).Dispose();
  }

}