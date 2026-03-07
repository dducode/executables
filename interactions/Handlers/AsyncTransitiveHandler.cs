using Interactions.Core;
using Interactions.Core.Handlers;

namespace Interactions.Handlers;

internal sealed class AsyncTransitiveHandler<T>(AsyncAction<T> action) : AsyncHandler<T, T> {

  protected override async ValueTask<T> ExecuteCore(T input, CancellationToken token = default) {
    await action(input, token);
    return input;
  }

}