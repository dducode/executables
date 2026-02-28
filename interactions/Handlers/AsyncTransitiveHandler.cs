using Interactions.Core;

namespace Interactions.Handlers;

internal sealed class AsyncTransitiveHandler<T>(AsyncAction<T> action) : AsyncHandler<T, T> {

  public override async ValueTask<T> Handle(T input, CancellationToken token = default) {
    ThrowIfDisposed(nameof(AsyncTransitiveHandler<T>));
    await action(input, token);
    return input;
  }

}