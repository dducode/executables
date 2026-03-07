using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Core.Lifecycle;

public static class DisposableExtensions {

  [Pure]
  public static IDisposable Compose(this IDisposable first, IDisposable second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositeDisposable(first, second);
  }

}