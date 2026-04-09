using System.Diagnostics.Contracts;
using Executables.Core.Executables;
using Executables.Internal;

namespace Executables;

public static partial class AsyncExecutable {

  /// <summary>
  /// Creates an executable that completes with the first finished executable.
  /// </summary>
  /// <param name="executables">Executables participating in the race.</param>
  /// <returns>Executable representing the race.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="executables"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="executables"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> Race<T1, T2>(params IAsyncExecutable<T1, T2>[] executables) {
    ExceptionsHelper.ThrowIfNullOrEmpty(executables, nameof(executables));
    return executables.Length > 1 ? new RaceExecutable<T1, T2>(executables) : executables[0];
  }

  /// <summary>
  /// Creates an executable that completes with the first finished delegate.
  /// </summary>
  /// <param name="functions">Delegates participating in the race.</param>
  /// <returns>Executable representing the race.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="functions"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="functions"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> Race<T1, T2>(params AsyncFunc<T1, T2>[] functions) {
    ExceptionsHelper.ThrowIfNullOrEmpty(functions, nameof(functions));
    return functions.Length > 1 ? Race(functions.Select(Create).ToArray()) : Create(functions[0]);
  }

  /// <summary>
  /// Creates a parameterless executable that completes with the first finished delegate.
  /// </summary>
  /// <param name="functions">Delegates participating in the race.</param>
  /// <returns>Executable representing the race.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="functions"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="functions"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<Unit, T> Race<T>(params AsyncFunc<T>[] functions) {
    ExceptionsHelper.ThrowIfNullOrEmpty(functions, nameof(functions));
    return functions.Length > 1 ? Race(functions.Select(Create).ToArray()) : Create(functions[0]);
  }

  /// <summary>
  /// Creates an executable that completes with the first finished action.
  /// </summary>
  /// <param name="actions">Actions participating in the race.</param>
  /// <returns>Executable representing the race.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="actions"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="actions"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T, Unit> Race<T>(params AsyncAction<T>[] actions) {
    ExceptionsHelper.ThrowIfNullOrEmpty(actions, nameof(actions));
    return actions.Length > 1 ? Race(actions.Select(Create).ToArray()) : Create(actions[0]);
  }

  /// <summary>
  /// Creates a parameterless executable that completes with the first finished action.
  /// </summary>
  /// <param name="actions">Actions participating in the race.</param>
  /// <returns>Executable representing the race.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="actions"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="actions"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<Unit, Unit> Race(params AsyncAction[] actions) {
    ExceptionsHelper.ThrowIfNullOrEmpty(actions, nameof(actions));
    return actions.Length > 1 ? Race(actions.Select(Create).ToArray()) : Create(actions[0]);
  }

  /// <summary>
  /// Creates an executable that completes with the first successfully finished executable.
  /// </summary>
  /// <param name="executables">Executables participating in the race.</param>
  /// <returns>Executable representing the success race.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="executables"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="executables"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> RaceSuccess<T1, T2>(params IAsyncExecutable<T1, T2>[] executables) {
    ExceptionsHelper.ThrowIfNullOrEmpty(executables, nameof(executables));
    return executables.Length > 1 ? new RaceSuccessExecutable<T1, T2>(executables) : executables[0];
  }

  /// <summary>
  /// Creates an executable that completes with the first successfully finished delegate.
  /// </summary>
  /// <param name="functions">Delegates participating in the success race.</param>
  /// <returns>Executable representing the success race.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="functions"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="functions"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> RaceSuccess<T1, T2>(params AsyncFunc<T1, T2>[] functions) {
    ExceptionsHelper.ThrowIfNullOrEmpty(functions, nameof(functions));
    return functions.Length > 1 ? RaceSuccess(functions.Select(Create).ToArray()) : Create(functions[0]);
  }

  /// <summary>
  /// Creates a parameterless executable that completes with the first successfully finished delegate.
  /// </summary>
  /// <param name="functions">Delegates participating in the success race.</param>
  /// <returns>Executable representing the success race.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="functions"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="functions"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<Unit, T> RaceSuccess<T>(params AsyncFunc<T>[] functions) {
    ExceptionsHelper.ThrowIfNullOrEmpty(functions, nameof(functions));
    return functions.Length > 1 ? RaceSuccess(functions.Select(Create).ToArray()) : Create(functions[0]);
  }

  /// <summary>
  /// Creates an executable that completes with the first successfully finished action.
  /// </summary>
  /// <param name="actions">Actions participating in the success race.</param>
  /// <returns>Executable representing the success race.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="actions"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="actions"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T, Unit> RaceSuccess<T>(params AsyncAction<T>[] actions) {
    ExceptionsHelper.ThrowIfNullOrEmpty(actions, nameof(actions));
    return actions.Length > 1 ? RaceSuccess(actions.Select(Create).ToArray()) : Create(actions[0]);
  }

  /// <summary>
  /// Creates a parameterless executable that completes with the first successfully finished action.
  /// </summary>
  /// <param name="actions">Actions participating in the success race.</param>
  /// <returns>Executable representing the success race.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="actions"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="actions"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<Unit, Unit> RaceSuccess(params AsyncAction[] actions) {
    ExceptionsHelper.ThrowIfNullOrEmpty(actions, nameof(actions));
    return actions.Length > 1 ? RaceSuccess(actions.Select(Create).ToArray()) : Create(actions[0]);
  }

}
