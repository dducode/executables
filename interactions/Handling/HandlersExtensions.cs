using System.Diagnostics.Contracts;
using Interactions.Core.Handlers;
using Interactions.Internal;

namespace Interactions.Handling;

public static partial class HandlersExtensions {

  [Pure]
  public static AsyncHandler<T1, T2> ToAsyncHandler<T1, T2>(this Handler<T1, T2> handler) {
    handler.ThrowIfNullReference();
    return new AsyncProxyHandler<T1, T2>(handler);
  }

  [Pure]
  public static Handler<T1, T2> OnDispose<T1, T2>(this Handler<T1, T2> handler, Action dispose) {
    handler.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(dispose, nameof(dispose));
    return new AnonymousDisposeHandler<T1, T2>(handler, dispose);
  }

  [Pure]
  public static AutoDisposeHandlerProvider<T1, T2> DisposeOnException<T1, T2>(this Handler<T1, T2> handler) {
    handler.ThrowIfNullReference();
    return new AutoDisposeHandlerProvider<T1, T2>(handler);
  }

}