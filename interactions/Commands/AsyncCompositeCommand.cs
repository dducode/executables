using Interactions.Core;
using Interactions.Core.Commands;

namespace Interactions.Commands;

internal sealed class AsyncCompositeCommand<T>(AsyncCommand<T> first, AsyncCommand<T> second) : AsyncCommand<T> {

  public override async ValueTask<bool> Execute(T input, CancellationToken token = default) {
    return await first.Execute(input, token) && await second.Execute(input, token);
  }

  public override IDisposable Handle(AsyncHandler<T, Unit> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return Handleable.MergedHandle(first, second, handler);
  }

}