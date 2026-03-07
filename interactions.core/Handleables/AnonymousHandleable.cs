using Interactions.Core.Handlers;
using Interactions.Core.Internal;

namespace Interactions.Core.Handleables;

internal sealed class AnonymousHandleable<T1, T2>(Func<Handler<T1, T2>, IDisposable> handling) : Handleable<T1, T2> {

  public override IDisposable Handle(Handler<T1, T2> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return handling(handler);
  }

}