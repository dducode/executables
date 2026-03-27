using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Core.Executables;

public static partial class ExecutableExtensions {

  /// <summary>
  /// Returns the same asynchronous executable instance.
  /// </summary>
  [Pure]
  public static IAsyncExecutable<T1, T2> AsExecutable<T1, T2>(this IAsyncExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return executable;
  }

  /// <summary>
  /// Converts an asynchronous executable to an asynchronous handler.
  /// </summary>
  /// <returns>Async handler wrapping the executable.</returns>
  [Pure]
  public static AsyncHandler<T1, T2> AsHandler<T1, T2>(this IAsyncExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return new AsyncExecutableHandler<T1, T2>(executable);
  }

  /// <summary>
  /// Returns the same asynchronous handler instance.
  /// </summary>
  /// <returns>The original async handler.</returns>
  [Pure]
  public static AsyncHandler<T1, T2> AsHandler<T1, T2>(this AsyncHandler<T1, T2> handler) {
    handler.ThrowIfNullReference();
    return handler;
  }

  /// <summary>
  /// Converts an asynchronous executable to an asynchronous query.
  /// </summary>
  /// <returns>Async query wrapping the executable.</returns>
  [Pure]
  public static IAsyncQuery<T1, T2> AsQuery<T1, T2>(this IAsyncExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return new AsyncExecutableQuery<T1, T2>(executable);
  }

  /// <summary>
  /// Returns the same asynchronous query instance.
  /// </summary>
  /// <returns>The original async query.</returns>
  [Pure]
  public static IAsyncQuery<T1, T2> AsQuery<T1, T2>(this IAsyncQuery<T1, T2> query) {
    query.ThrowIfNullReference();
    return query;
  }

  /// <summary>
  /// Converts an asynchronous executable to an asynchronous command.
  /// </summary>
  /// <returns>Async command wrapping the executable.</returns>
  [Pure]
  public static IAsyncCommand<T> AsCommand<T>(this IAsyncExecutable<T, bool> executable) {
    executable.ThrowIfNullReference();
    return new AsyncExecutableCommand<T>(executable);
  }

  /// <summary>
  /// Returns the same asynchronous command instance.
  /// </summary>
  /// <returns>The original async command.</returns>
  [Pure]
  public static IAsyncCommand<T> AsCommand<T>(this IAsyncCommand<T> command) {
    command.ThrowIfNullReference();
    return command;
  }

}