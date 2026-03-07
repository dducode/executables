using System.Diagnostics.Contracts;

namespace Interactions.Core.Handlers;

public abstract partial class AsyncHandler<T1, T2> {

  [Pure]
  public AsyncHandler<T1, T2> DisposeOnException<TEx>(IDisposable handle) where TEx : Exception {
    ExceptionsHelper.ThrowIfNull(handle, nameof(handle));
    return new AsyncAutoDisposeHandler<T1, T2, TEx>(this, handle);
  }

}