using System.Diagnostics.Contracts;
using Executables.Core.Commands;
using Executables.Internal;

namespace Executables.Commands;

public static class AsyncCommandsExtensions {

  /// <summary>
  /// Executes a parameterless asynchronous command.
  /// </summary>
  /// <returns>
  /// Task that resolves to <see langword="true"/> if execution succeeded; otherwise, <see langword="false"/>.
  /// </returns>
  public static ValueTask<bool> Execute(this IAsyncCommand<Unit> command, CancellationToken token = default) {
    return command.Execute(default, token);
  }

  /// <summary>
  /// Composes two asynchronous commands into a single asynchronous command.
  /// </summary>
  /// <returns>Asynchronous composite command.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncCommand<T> Compose<T>(this IAsyncCommand<T> command, IAsyncCommand<T> other) {
    command.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new AsyncCompositeCommand<T>(command, other);
  }

  /// <summary>
  /// Composes an asynchronous command with a synchronous command.
  /// </summary>
  /// <returns>Asynchronous composite command.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncCommand<T> Compose<T>(this IAsyncCommand<T> command, ICommand<T> other) {
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return command.Compose(other.ToAsyncCommand());
  }

}