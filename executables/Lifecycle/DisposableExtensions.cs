using System.Diagnostics.Contracts;
using Executables.Core.Lifecycle;
using Executables.Internal;

namespace Executables.Lifecycle;

public static class DisposableExtensions {

  [Pure]
  public static IDisposable Compose(this IDisposable first, IDisposable second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositeDisposable(first, second);
  }

}