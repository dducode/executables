using System.Diagnostics.Contracts;
using Executables.Core.Handlers;
using Executables.Internal;

namespace Executables.Handling;

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
  public static Handler<T1, T2> DisposeOnUnhandledException<T1, T2>(this Handler<T1, T2> handler) {
    handler.ThrowIfNullReference();
    return new AutoDisposedHandler<T1, T2>(handler);
  }

}