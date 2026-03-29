using System.Diagnostics.Contracts;
using Executables.Core.Handlers;
using Executables.Internal;

namespace Executables.Handling;

public static partial class HandlersExtensions {

  [Pure]
  public static AsyncHandler<T1, T2> OnDispose<T1, T2>(this AsyncHandler<T1, T2> handler, Action dispose) {
    handler.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(dispose, nameof(dispose));
    return new AsyncAnonymousDisposeHandler<T1, T2>(handler, dispose);
  }

  [Pure]
  public static AsyncHandler<T1, T2> DisposeOnUnhandledException<T1, T2>(this AsyncHandler<T1, T2> handler) {
    handler.ThrowIfNullReference();
    return new AsyncAutoDisposedHandler<T1, T2>(handler);
  }

}