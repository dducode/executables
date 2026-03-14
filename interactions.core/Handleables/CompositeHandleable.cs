using Interactions.Core.Internal;

namespace Interactions.Core.Handleables;

internal sealed class CompositeHandleable<T1, T2, THandler>(
  IHandleable<T1, T2, THandler> first,
  IHandleable<T1, T2, THandler> second) : IHandleable<T1, T2, THandler> where THandler : Handler<T1, T2> {

  IDisposable IHandleable<T1, T2, THandler>.Handle(THandler handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return Handleable.MergedHandle(first, second, handler);
  }

}