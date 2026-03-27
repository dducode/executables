using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Core.Executables;

public static partial class ExecutableExtensions {

  /// <summary>
  /// Returns the same executable instance.
  /// </summary>
  [Pure]
  public static IExecutable<T1, T2> AsExecutable<T1, T2>(this IExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return executable;
  }

  /// <summary>
  /// Converts a synchronous executable to an asynchronous executable.
  /// </summary>
  /// <returns>Asynchronous proxy executable.</returns>
  [Pure]
  public static IAsyncExecutable<T1, T2> ToAsyncExecutable<T1, T2>(this IExecutable<T1, T2> inner) {
    inner.ThrowIfNullReference();
    return new AsyncProxyExecutable<T1, T2>(inner);
  }

  /// <summary>
  /// Converts an executable to a handler.
  /// </summary>
  /// <returns>Handler wrapping the executable.</returns>
  [Pure]
  public static Handler<T1, T2> AsHandler<T1, T2>(this IExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return new ExecutableHandler<T1, T2>(executable);
  }

  /// <summary>
  /// Returns the same handler instance.
  /// </summary>
  /// <returns>The original handler.</returns>
  [Pure]
  public static Handler<T1, T2> AsHandler<T1, T2>(this Handler<T1, T2> handler) {
    handler.ThrowIfNullReference();
    return handler;
  }

  /// <summary>
  /// Converts an executable to a query.
  /// </summary>
  /// <returns>Query wrapping the executable.</returns>
  [Pure]
  public static IQuery<T1, T2> AsQuery<T1, T2>(this IExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return new ExecutableQuery<T1, T2>(executable);
  }

  /// <summary>
  /// Returns the same query instance.
  /// </summary>
  /// <returns>The original query.</returns>
  [Pure]
  public static IQuery<T1, T2> AsQuery<T1, T2>(this IQuery<T1, T2> query) {
    query.ThrowIfNullReference();
    return query;
  }

  /// <summary>
  /// Converts an executable to a command.
  /// </summary>
  /// <returns>Command wrapping the executable.</returns>
  [Pure]
  public static ICommand<T> AsCommand<T>(this IExecutable<T, bool> executable) {
    executable.ThrowIfNullReference();
    return new ExecutableCommand<T>(executable);
  }

  /// <summary>
  /// Returns the same command instance.
  /// </summary>
  /// <returns>The original command.</returns>
  [Pure]
  public static ICommand<T> AsCommand<T>(this ICommand<T> command) {
    command.ThrowIfNullReference();
    return command;
  }

}