using Interactions.Core.Handlers;

namespace Interactions.Core.Handleables;

internal sealed class AsyncAnonymousHandleable<T1, T2>(Func<AsyncHandler<T1, T2>, IDisposable> handling) : AsyncHandleable<T1, T2> {

  public override IDisposable Handle(AsyncHandler<T1, T2> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return handling(handler);
  }

}