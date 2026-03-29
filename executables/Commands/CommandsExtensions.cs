using System.Diagnostics.Contracts;
using Executables.Core.Commands;
using Executables.Internal;

namespace Executables.Commands;

public static partial class CommandsExtensions {

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
  /// Composes two commands into a single command.
  /// </summary>
  /// <returns>Composite command that executes both commands.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
  [Pure]
  public static ICommand<T> Compose<T>(this ICommand<T> command, ICommand<T> other) {
    command.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new CompositeCommand<T>(command, other);
  }

  /// <summary>
  /// Composes a synchronous command with an asynchronous command.
  /// </summary>
  /// <returns>Asynchronous composite command.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncCommand<T> Compose<T>(this ICommand<T> command, IAsyncCommand<T> other) {
    return command.ToAsyncCommand().Compose(other);
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