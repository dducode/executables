using System.Diagnostics.Contracts;
using Executables.Core.Commands;
using Executables.Internal;

namespace Executables.Commands;

/// <summary>
/// Extension methods for asynchronous commands.
/// </summary>
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
  /// Prepends an asynchronous command to the current asynchronous command.
  /// </summary>
  /// <param name="first">Command executed only if <paramref name="second"/> succeeds.</param>
  /// <param name="second">Command executed first.</param>
  /// <returns>Asynchronous command that returns <see langword="true"/> only when <paramref name="second"/> and then <paramref name="first"/> both succeed.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncCommand<T> Prepend<T>(this IAsyncCommand<T> first, IAsyncCommand<T> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new AsyncCompositeCommand<T>(first, second);
  }

  /// <summary>
  /// Appends an asynchronous command to the current asynchronous command.
  /// </summary>
  /// <param name="first">Command executed first.</param>
  /// <param name="second">Command executed only if <paramref name="first"/> succeeds.</param>
  /// <returns>Asynchronous command that returns <see langword="true"/> only when <paramref name="first"/> and then <paramref name="second"/> both succeed.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncCommand<T> Append<T>(this IAsyncCommand<T> first, IAsyncCommand<T> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return second.Prepend(first);
  }

  /// <summary>
  /// Prepends a synchronous command to the current asynchronous command.
  /// </summary>
  /// <param name="first">Asynchronous command executed only if <paramref name="second"/> succeeds.</param>
  /// <param name="second">Synchronous command executed first.</param>
  /// <returns>Asynchronous command that returns <see langword="true"/> only when <paramref name="second"/> and then <paramref name="first"/> both succeed.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncCommand<T> Prepend<T>(this IAsyncCommand<T> first, ICommand<T> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return first.Prepend(second.ToAsyncCommand());
  }

  /// <summary>
  /// Appends a synchronous command to the current asynchronous command.
  /// </summary>
  /// <param name="first">Asynchronous command executed first.</param>
  /// <param name="second">Synchronous command executed only if <paramref name="first"/> succeeds.</param>
  /// <returns>Asynchronous command that returns <see langword="true"/> only when <paramref name="first"/> and then <paramref name="second"/> both succeed.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncCommand<T> Append<T>(this IAsyncCommand<T> first, ICommand<T> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return second.ToAsyncCommand().Prepend(first);
  }

}