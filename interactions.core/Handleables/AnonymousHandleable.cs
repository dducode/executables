using Interactions.Core.Internal;

namespace Interactions.Core.Handleables;

internal sealed class AnonymousHandleable<T1, T2>(Func<Handler<T1, T2>, IDisposable> handling) : IHandleable<T1, T2> {

  public IDisposable Handle(Handler<T1, T2> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return handling(handler);
  }

}