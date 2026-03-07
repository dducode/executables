using System.Diagnostics.Contracts;
using Interactions.Core.Internal;
using Interactions.Core.Lifecycle;

namespace Interactions.Core;

public static class Disposable {

  [Pure]
  public static IDisposable Create(Action dispose) {
    ExceptionsHelper.ThrowIfNull(dispose, nameof(dispose));
    return new AnonymousDisposable(dispose);
  }

}