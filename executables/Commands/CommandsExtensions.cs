using System.Diagnostics.Contracts;
using Executables.Core.Commands;
using Executables.Internal;

namespace Executables.Commands;

/// <summary>
/// Extension methods for commands.
/// </summary>
public static class CommandsExtensions {

  /// <summary>
  /// Executes a parameterless command.
  /// </summary>
  /// <returns>
  /// <see langword="true"/> if execution succeeded; otherwise, <see langword="false"/>.
  /// </returns>
  public static bool Execute(this ICommand<Unit> command) {
    return command.Execute(default);
  }

  /// <summary>
  /// Prepends a command to the current command.
  /// </summary>
  /// <param name="first">Command executed only if <paramref name="second"/> succeeds.</param>
  /// <param name="second">Command executed first.</param>
  /// <returns>Command that returns <see langword="true"/> only when both commands succeed.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static ICommand<T> Prepend<T>(this ICommand<T> first, ICommand<T> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositeCommand<T>(first, second);
  }

  /// <summary>
  /// Appends a command to the current command.
  /// </summary>
  /// <param name="first">Command executed first.</param>
  /// <param name="second">Command executed only if <paramref name="first"/> succeeds.</param>
  /// <returns>Command that returns <see langword="true"/> only when both commands succeed.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static ICommand<T> Append<T>(this ICommand<T> first, ICommand<T> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return second.Prepend(first);
  }

  /// <summary>
  /// Prepends an asynchronous command to the current synchronous command.
  /// </summary>
  /// <param name="first">Synchronous command executed only if <paramref name="second"/> succeeds.</param>
  /// <param name="second">Asynchronous command executed first.</param>
  /// <returns>Asynchronous command that returns <see langword="true"/> only when both commands succeed.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncCommand<T> Prepend<T>(this ICommand<T> first, IAsyncCommand<T> second) {
    return first.ToAsyncCommand().Prepend(second);
  }

  /// <summary>
  /// Appends an asynchronous command to the current synchronous command.
  /// </summary>
  /// <param name="first">Command executed first.</param>
  /// <param name="second">Asynchronous command executed only if <paramref name="first"/> succeeds.</param>
  /// <returns>Asynchronous command that returns <see langword="true"/> only when both commands succeed.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncCommand<T> Append<T>(this ICommand<T> first, IAsyncCommand<T> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return second.Prepend(first.ToAsyncCommand());
  }

  /// <summary>
  /// Converts a synchronous command into an asynchronous command.
  /// </summary>
  /// <returns>Asynchronous command proxy.</returns>
  [Pure]
  public static IAsyncCommand<T> ToAsyncCommand<T>(this ICommand<T> command) {
    command.ThrowIfNullReference();
    return new AsyncProxyCommand<T>(command);
  }

}