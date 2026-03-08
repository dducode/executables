using System.Diagnostics.Contracts;
using Interactions.Core.Handleables;
using Interactions.Core.Internal;

namespace Interactions.Core;

public static class AsyncHandleable {

  [Pure]
  public static IAsyncHandleable<T1, T2> Create<T1, T2>(Func<AsyncHandler<T1, T2>, IDisposable> handling) {
    ExceptionsHelper.ThrowIfNull(handling, nameof(handling));
    return new AsyncAnonymousHandleable<T1, T2>(handling);
  }

}