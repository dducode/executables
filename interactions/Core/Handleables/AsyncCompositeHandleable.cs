using Interactions.Handling;
using Interactions.Internal;

namespace Interactions.Core.Handleables;

internal sealed class AsyncCompositeHandleable<T1, T2, THandler>(
  IAsyncHandleable<T1, T2, THandler> first,
  IAsyncHandleable<T1, T2, THandler> second) : IAsyncHandleable<T1, T2, THandler> where THandler : AsyncHandler<T1, T2> {

  IDisposable IAsyncHandleable<T1, T2, THandler>.Handle(THandler handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return Handleable.MergedHandle(first, second, handler);
  }

}