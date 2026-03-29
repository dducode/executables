using System.Diagnostics.Contracts;
using Executables.Core.Lifecycle;
using Executables.Internal;

namespace Executables.Lifecycle;

public static class Disposable {

  [Pure]
  public static IDisposable Create(Action dispose) {
    ExceptionsHelper.ThrowIfNull(dispose, nameof(dispose));
    return new AnonymousDisposable(dispose);
  }

}