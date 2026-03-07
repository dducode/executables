using Interactions.Core.Internal;

namespace Interactions.Core.Handleables;

internal sealed class MergedHandleable<T1, T2>(Handleable<T1, T2> first, Handleable<T1, T2> second) : Handleable<T1, T2> {

  public override IDisposable Handle(Handler<T1, T2> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return Handleable.MergedHandle(first, second, handler);
  }

}