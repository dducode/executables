using System.Diagnostics.Contracts;
using Interactions.Core.Executables;

namespace Interactions.Core.Handlers;

/// <summary>
/// Factory methods for creating <see cref="AsyncHandler{T1,T2}"/>.
/// <example>
/// <code>
/// var asyncHandler = AsyncHandler.Create(async (int i, CancellationToken ct) => {
///   await Task.Delay(10, ct);
///   return i.ToString();
/// });
/// </code>
/// </example>
/// </summary>
public static class AsyncHandler {

  /// <summary>
  /// Creates an async handler from an async function.
  /// </summary>
  /// <typeparam name="T1">Input type.</typeparam>
  /// <typeparam name="T2">Output type.</typeparam>
  /// <param name="func">Async function used for handling.</param>
  [Pure]
  public static AsyncHandler<T1, T2> Create<T1, T2>(AsyncFunc<T1, T2> func) {
    return AsyncExecutable.Create(func).AsHandler();
  }

  /// <summary>
  /// Creates an async handler from a parameterless async function.
  /// </summary>
  /// <typeparam name="T">Output type.</typeparam>
  /// <param name="func">Async function used for handling.</param>
  [Pure]
  public static AsyncHandler<Unit, T> Create<T>(AsyncFunc<T> func) {
    return AsyncExecutable.Create(func).AsHandler();
  }

  /// <summary>
  /// Creates an async handler from an async action.
  /// </summary>
  /// <typeparam name="T">Input type.</typeparam>
  /// <param name="action">Async action used for handling.</param>
  [Pure]
  public static AsyncHandler<T, Unit> Create<T>(AsyncAction<T> action) {
    return AsyncExecutable.Create(action).AsHandler();
  }

  /// <summary>
  /// Creates an async handler from a parameterless async action.
  /// </summary>
  /// <param name="action">Async action used for handling.</param>
  [Pure]
  public static AsyncHandler<Unit, Unit> Create(AsyncAction action) {
    return AsyncExecutable.Create(action).AsHandler();
  }

}