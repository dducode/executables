using System.Diagnostics.Contracts;
using Interactions.Core.Handlers;

namespace Interactions.Core.Extensions;

public static class HandlersExtensions {

  [Pure]
  public static AsyncHandler<T1, T2> ToAsyncHandler<T1, T2>(this Handler<T1, T2> handler) {
    ExceptionsHelper.ThrowIfNullReference(handler);
    return new AsyncProxyHandler<T1, T2>(handler);
  }

}