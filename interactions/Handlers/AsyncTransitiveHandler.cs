using Interactions.Core;

namespace Interactions.Handlers;

internal sealed class AsyncTransitiveHandler<T>(AsyncAction<T> action) : AsyncHandler<T, T> {

  protected override async ValueTask<T> HandleCore(T input, CancellationToken token = default) {
    await action(input, token);
    return input;
  }

}