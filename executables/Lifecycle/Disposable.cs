using System.Diagnostics.Contracts;
using Executables.Core.Lifecycle;
using Executables.Internal;

namespace Executables.Lifecycle;

/// <summary>
/// Factory methods for creating disposable objects.
/// </summary>
public static class Disposable {

  /// <summary>
  /// Creates a disposable object from a disposal action.
  /// </summary>
  /// <param name="dispose">Action executed when the created disposable is disposed.</param>
  /// <returns>Disposable object that delegates disposal to <paramref name="dispose"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="dispose"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IDisposable Create(Action dispose) {
    ExceptionsHelper.ThrowIfNull(dispose, nameof(dispose));
    return new AnonymousDisposable(dispose);
  }

}