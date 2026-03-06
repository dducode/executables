using System.Diagnostics.Contracts;
using Interactions.Core.Handlers;

namespace Interactions.Core;

public static class AsyncHandler {

  /// <summary>
  /// Creates an async handler from an async function.
  /// </summary>
  /// <typeparam name="T1">Input type.</typeparam>
  /// <typeparam name="T2">Output type.</typeparam>
  /// <param name="func">Async function used for handling.</param>
  [Pure]
  public static AsyncHandler<T1, T2> FromMethod<T1, T2>(AsyncFunc<T1, T2> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new AsyncAnonymousHandler<T1, T2>(func);
  }

  /// <summary>
  /// Creates an async handler from a parameterless async function.
  /// </summary>
  /// <typeparam name="T">Output type.</typeparam>
  /// <param name="action">Async function used for handling.</param>
  [Pure]
  public static AsyncHandler<Unit, T> FromMethod<T>(AsyncFunc<T> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return FromMethod<Unit, T>((_, token) => action(token));
  }

  /// <summary>
  /// Creates an async handler from an async action.
  /// </summary>
  /// <typeparam name="T">Input type.</typeparam>
  /// <param name="action">Async action used for handling.</param>
  [Pure]
  public static AsyncHandler<T, Unit> FromMethod<T>(AsyncAction<T> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return FromMethod<T, Unit>(async (input, token) => {
      await action(input, token);
      return default;
    });
  }

  /// <summary>
  /// Creates an async handler from a parameterless async action.
  /// </summary>
  /// <param name="action">Async action used for handling.</param>
  [Pure]
  public static AsyncHandler<Unit, Unit> FromMethod(AsyncAction action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return FromMethod<Unit, Unit>(async (_, token) => {
      await action(token);
      return default;
    });
  }

}