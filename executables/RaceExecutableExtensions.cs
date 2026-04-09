using System.Diagnostics.Contracts;
using Executables.Core.Executables;
using Executables.Internal;

namespace Executables;

/// <summary>
/// Extension methods for building race-style executable compositions.
/// </summary>
public static class RaceExecutableExtensions {

  /// <summary>
  /// Races multiple asynchronous executables and returns the first completed result.
  /// </summary>
  /// <remarks>
  /// If only one executable is provided, it is composed directly without creating a race wrapper.
  /// </remarks>
  /// <param name="executable">Executable that produces the shared race input.</param>
  /// <param name="executables">Executables competing for the first result.</param>
  /// <returns>Executable that returns the first completed result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="executables"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="executables"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Race<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, params IAsyncExecutable<T2, T3>[] executables) {
    ExceptionsHelper.ThrowIfNullOrEmpty(executables, nameof(executables));
    return executables.Length > 1 ? executable.Then(new RaceExecutable<T2, T3>(executables)) : executable.Then(executables[0]);
  }

  /// <summary>
  /// Races multiple asynchronous delegates and returns the first completed result.
  /// </summary>
  /// <remarks>
  /// If only one delegate is provided, it is composed directly without creating a race wrapper.
  /// </remarks>
  /// <param name="executable">Executable that produces the shared race input.</param>
  /// <param name="functions">Delegates competing for the first result.</param>
  /// <returns>Executable that returns the first completed result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="functions"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="functions"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Race<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, params AsyncFunc<T2, T3>[] functions) {
    ExceptionsHelper.ThrowIfNullOrEmpty(functions, nameof(functions));
    return functions.Length > 1 ? executable.Race(functions.Select(AsyncExecutable.Create).ToArray()) : executable.Then(functions[0]);
  }

  /// <summary>
  /// Races multiple parameterless asynchronous delegates after the source executable completes and returns the first completed result.
  /// </summary>
  /// <remarks>
  /// If only one delegate is provided, it is composed directly without creating a race wrapper.
  /// </remarks>
  /// <param name="executable">Executable that produces the race trigger.</param>
  /// <param name="functions">Delegates competing for the first result.</param>
  /// <returns>Executable that returns the first completed result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="functions"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="functions"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> Race<T1, T2>(this IAsyncExecutable<T1, Unit> executable, params AsyncFunc<T2>[] functions) {
    ExceptionsHelper.ThrowIfNullOrEmpty(functions, nameof(functions));
    return functions.Length > 1 ? executable.Race(functions.Select(AsyncExecutable.Create).ToArray()) : executable.Then(functions[0]);
  }

  /// <summary>
  /// Races multiple asynchronous actions after the source executable completes and returns when the first action completes.
  /// </summary>
  /// <remarks>
  /// If only one action is provided, it is composed directly without creating a race wrapper.
  /// </remarks>
  /// <param name="executable">Executable that produces the shared race input.</param>
  /// <param name="actions">Actions competing for the first completion.</param>
  /// <returns>Executable that completes when the first action completes.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="actions"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="actions"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T1, Unit> Race<T1, T2>(this IAsyncExecutable<T1, T2> executable, params AsyncAction<T2>[] actions) {
    ExceptionsHelper.ThrowIfNullOrEmpty(actions, nameof(actions));
    return actions.Length > 1 ? executable.Race(actions.Select(AsyncExecutable.Create).ToArray()) : executable.Then(actions[0]);
  }

  /// <summary>
  /// Races multiple parameterless asynchronous actions after the source executable completes and returns when the first action completes.
  /// </summary>
  /// <remarks>
  /// If only one action is provided, it is composed directly without creating a race wrapper.
  /// </remarks>
  /// <param name="executable">Executable that produces the race trigger.</param>
  /// <param name="actions">Actions competing for the first completion.</param>
  /// <returns>Executable that completes when the first action completes.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="actions"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="actions"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T, Unit> Race<T>(this IAsyncExecutable<T, Unit> executable, params AsyncAction[] actions) {
    ExceptionsHelper.ThrowIfNullOrEmpty(actions, nameof(actions));
    return actions.Length > 1 ? executable.Race(actions.Select(AsyncExecutable.Create).ToArray()) : executable.Then(actions[0]);
  }

  /// <summary>
  /// Races multiple asynchronous executables and returns the first successful result.
  /// </summary>
  /// <remarks>
  /// If only one executable is provided, it is composed directly without creating a race-success wrapper.
  /// </remarks>
  /// <param name="executable">Executable that produces the shared race input.</param>
  /// <param name="executables">Executables competing for the first successful result.</param>
  /// <returns>Executable that returns the first successful result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="executables"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="executables"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> RaceSuccess<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, params IAsyncExecutable<T2, T3>[] executables) {
    ExceptionsHelper.ThrowIfNullOrEmpty(executables, nameof(executables));
    return executables.Length > 1 ? executable.Then(new RaceSuccessExecutable<T2, T3>(executables)) : executable.Then(executables[0]);
  }

  /// <summary>
  /// Races multiple asynchronous delegates and returns the first successful result.
  /// </summary>
  /// <remarks>
  /// If only one delegate is provided, it is composed directly without creating a race-success wrapper.
  /// </remarks>
  /// <param name="executable">Executable that produces the shared race input.</param>
  /// <param name="functions">Delegates competing for the first successful result.</param>
  /// <returns>Executable that returns the first successful result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="functions"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="functions"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> RaceSuccess<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, params AsyncFunc<T2, T3>[] functions) {
    ExceptionsHelper.ThrowIfNullOrEmpty(functions, nameof(functions));
    return functions.Length > 1 ? executable.RaceSuccess(functions.Select(AsyncExecutable.Create).ToArray()) : executable.Then(functions[0]);
  }

  /// <summary>
  /// Races multiple parameterless asynchronous delegates after the source executable completes and returns the first successful result.
  /// </summary>
  /// <remarks>
  /// If only one delegate is provided, it is composed directly without creating a race-success wrapper.
  /// </remarks>
  /// <param name="executable">Executable that produces the race trigger.</param>
  /// <param name="functions">Delegates competing for the first successful result.</param>
  /// <returns>Executable that returns the first successful result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="functions"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="functions"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> RaceSuccess<T1, T2>(this IAsyncExecutable<T1, Unit> executable, params AsyncFunc<T2>[] functions) {
    ExceptionsHelper.ThrowIfNullOrEmpty(functions, nameof(functions));
    return functions.Length > 1 ? executable.RaceSuccess(functions.Select(AsyncExecutable.Create).ToArray()) : executable.Then(functions[0]);
  }

  /// <summary>
  /// Races multiple asynchronous actions after the source executable completes and returns when the first action completes successfully.
  /// </summary>
  /// <remarks>
  /// If only one action is provided, it is composed directly without creating a race-success wrapper.
  /// </remarks>
  /// <param name="executable">Executable that produces the shared race input.</param>
  /// <param name="actions">Actions competing for the first successful completion.</param>
  /// <returns>Executable that completes when the first action completes successfully.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="actions"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="actions"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T1, Unit> RaceSuccess<T1, T2>(this IAsyncExecutable<T1, T2> executable, params AsyncAction<T2>[] actions) {
    ExceptionsHelper.ThrowIfNullOrEmpty(actions, nameof(actions));
    return actions.Length > 1 ? executable.RaceSuccess(actions.Select(AsyncExecutable.Create).ToArray()) : executable.Then(actions[0]);
  }

  /// <summary>
  /// Races multiple parameterless asynchronous actions after the source executable completes and returns when the first action completes successfully.
  /// </summary>
  /// <remarks>
  /// If only one action is provided, it is composed directly without creating a race-success wrapper.
  /// </remarks>
  /// <param name="executable">Executable that produces the race trigger.</param>
  /// <param name="actions">Actions competing for the first successful completion.</param>
  /// <returns>Executable that completes when the first action completes successfully.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="actions"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="actions"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T, Unit> RaceSuccess<T>(this IAsyncExecutable<T, Unit> executable, params AsyncAction[] actions) {
    ExceptionsHelper.ThrowIfNullOrEmpty(actions, nameof(actions));
    return actions.Length > 1 ? executable.RaceSuccess(actions.Select(AsyncExecutable.Create).ToArray()) : executable.Then(actions[0]);
  }

}