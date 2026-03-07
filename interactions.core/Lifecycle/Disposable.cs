using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Core.Lifecycle;

public static class Disposable {

  [Pure]
  public static IDisposable Create(Action dispose) {
    ExceptionsHelper.ThrowIfNull(dispose, nameof(dispose));
    return new AnonymousDisposable(dispose);
  }

}