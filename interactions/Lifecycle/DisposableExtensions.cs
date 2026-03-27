using System.Diagnostics.Contracts;
using Interactions.Core.Lifecycle;
using Interactions.Internal;

namespace Interactions.Lifecycle;

public static class DisposableExtensions {

  [Pure]
  public static IDisposable Compose(this IDisposable first, IDisposable second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositeDisposable(first, second);
  }

}