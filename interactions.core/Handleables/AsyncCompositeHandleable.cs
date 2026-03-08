using Interactions.Core.Internal;

namespace Interactions.Core.Handleables;

internal sealed class AsyncCompositeHandleable<T1, T2, THandler>(
  IAsyncHandleable<T1, T2, THandler> first,
  IAsyncHandleable<T1, T2, THandler> second) : IAsyncHandleable<T1, T2, THandler> where THandler : AsyncHandler<T1, T2> {

  public IDisposable Handle(THandler handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return Handleable.MergedHandle(first, second, handler);
  }

}