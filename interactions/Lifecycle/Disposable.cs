using System.Diagnostics.Contracts;
using Interactions.Core.Lifecycle;
using Interactions.Internal;

namespace Interactions.Lifecycle;

public static class Disposable {

  [Pure]
  public static IDisposable Create(Action dispose) {
    ExceptionsHelper.ThrowIfNull(dispose, nameof(dispose));
    return new AnonymousDisposable(dispose);
  }

}