using System.Diagnostics.Contracts;
using Executables.Core.Lifecycle;
using Executables.Internal;

namespace Executables.Lifecycle;

/// <summary>
/// Extension methods for disposable objects.
/// </summary>
public static class DisposableExtensions {

  /// <summary>
  /// Combines two disposable objects into one disposable that disposes both.
  /// </summary>
  /// <param name="first">First disposable to dispose.</param>
  /// <param name="second">Second disposable to dispose.</param>
  /// <returns>Composite disposable that disposes both inputs.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IDisposable Compose(this IDisposable first, IDisposable second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositeDisposable(first, second);
  }

}