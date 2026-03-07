using Interactions.Core.Internal;

namespace Interactions.Core.Handleables;

internal sealed class AsyncMergedHandleable<T1, T2>(AsyncHandleable<T1, T2> first, AsyncHandleable<T1, T2> second) : AsyncHandleable<T1, T2> {

  public override IDisposable Handle(AsyncHandler<T1, T2> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return Handleable.MergedHandle(first, second, handler);
  }

}