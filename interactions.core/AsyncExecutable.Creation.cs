using System.Diagnostics.Contracts;
using Interactions.Core.Executables;
using Interactions.Core.Internal;

namespace Interactions.Core;

public static class AsyncExecutable {

  /// <summary>
  /// Creates an async executable from an async function.
  /// </summary>
  /// <typeparam name="T1">Input type.</typeparam>
  /// <typeparam name="T2">Output type.</typeparam>
  /// <param name="func">Async function used for execution.</param>
  [Pure]
  public static IAsyncExecutable<T1, T2> Create<T1, T2>(AsyncFunc<T1, T2> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new ExecutableAsyncFunc<T1, T2>(func);
  }

  /// <summary>
  /// Creates an async executable from a parameterless async function.
  /// </summary>
  /// <typeparam name="T">Output type.</typeparam>
  /// <param name="func">Async function used for execution.</param>
  [Pure]
  public static IAsyncExecutable<Unit, T> Create<T>(AsyncFunc<T> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new ExecutableAsyncFunc<T>(func);
  }

  /// <summary>
  /// Creates an async executable from an async action.
  /// </summary>
  /// <typeparam name="T">Input type.</typeparam>
  /// <param name="action">Async action used for execution.</param>
  [Pure]
  public static IAsyncExecutable<T, Unit> Create<T>(AsyncAction<T> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new ExecutableAsyncAction<T>(action);
  }

  /// <summary>
  /// Creates an async executable from a parameterless async action.
  /// </summary>
  /// <param name="action">Async action used for execution.</param>
  [Pure]
  public static IAsyncExecutable<Unit, Unit> Create(AsyncAction action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new ExecutableAsyncAction(action);
  }

}