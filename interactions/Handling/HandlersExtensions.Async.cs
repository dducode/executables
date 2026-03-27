using System.Diagnostics.Contracts;
using Interactions.Core.Handlers;
using Interactions.Internal;

namespace Interactions.Handling;

public static partial class HandlersExtensions {

  [Pure]
  public static AsyncHandler<T1, T2> OnDispose<T1, T2>(this AsyncHandler<T1, T2> handler, Action dispose) {
    handler.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(dispose, nameof(dispose));
    return new AsyncAnonymousDisposeHandler<T1, T2>(handler, dispose);
  }

  [Pure]
  public static AsyncAutoDisposeHandlerProvider<T1, T2> DisposeExternalHandle<T1, T2>(this AsyncHandler<T1, T2> handler, IDisposable handle) {
    handler.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(handle, nameof(handle));
    return new AsyncAutoDisposeHandlerProvider<T1, T2>(handler, handle);
  }

}