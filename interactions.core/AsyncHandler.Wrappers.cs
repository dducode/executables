using System.Diagnostics.Contracts;
using Interactions.Core.Handlers;
using Interactions.Core.Internal;

namespace Interactions.Core;

public abstract partial class AsyncHandler<T1, T2> {

  [Pure]
  public AsyncHandler<T1, T2> DisposeOnException<TEx>(IDisposable handle) where TEx : Exception {
    ExceptionsHelper.ThrowIfNull(handle, nameof(handle));
    return new AsyncAutoDisposeHandler<T1, T2, TEx>(this, handle);
  }

}