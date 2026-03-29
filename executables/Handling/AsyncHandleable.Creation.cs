using System.Diagnostics.Contracts;
using Executables.Core.Handleables;
using Executables.Internal;

namespace Executables.Handling;

public static class AsyncHandleable {

  [Pure]
  public static IAsyncHandleable<T1, T2> Create<T1, T2>(Func<AsyncHandler<T1, T2>, IDisposable> handling) {
    ExceptionsHelper.ThrowIfNull(handling, nameof(handling));
    return new AsyncAnonymousHandleable<T1, T2>(handling);
  }

}