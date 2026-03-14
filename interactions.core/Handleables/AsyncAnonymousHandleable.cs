using Interactions.Core.Internal;

namespace Interactions.Core.Handleables;

internal sealed class AsyncAnonymousHandleable<T1, T2>(Func<AsyncHandler<T1, T2>, IDisposable> handling) : IAsyncHandleable<T1, T2> {

  IDisposable IAsyncHandleable<T1, T2, AsyncHandler<T1, T2>>.Handle(AsyncHandler<T1, T2> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return handling(handler);
  }

}