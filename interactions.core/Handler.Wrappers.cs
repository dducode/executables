using System.Diagnostics.Contracts;
using Interactions.Core.Handlers;

namespace Interactions.Core;

public abstract partial class Handler<T1, T2> {

  [Pure]
  public Handler<T1, T2> DisposeOnException<TEx>(IDisposable handle) where TEx : Exception {
    ExceptionsHelper.ThrowIfNull(handle, nameof(handle));
    return new AutoDisposeHandler<T1, T2, TEx>(this, handle);
  }

}