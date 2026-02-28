using System.Diagnostics.Contracts;
using Interactions.Core.Handlers;

namespace Interactions.Core;

public abstract partial class AsyncHandler<T1, T2> {

  [Pure]
  public AsyncHandler<T1, T2> DisposeOnException<TException>(IDisposable handle) where TException : Exception {
    ExceptionsHelper.ThrowIfNull(handle, nameof(handle));
    return new AsyncAutoDisposeHandler<T1, T2, TException>(this, handle);
  }

}