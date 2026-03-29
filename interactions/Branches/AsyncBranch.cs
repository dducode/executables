using Interactions.Internal;

namespace Interactions.Branches;

/// <summary>
/// Factory methods for creating asynchronous conditional branch builders with input and output values.
/// </summary>
public static class AsyncBranch<T1, T2> {

  /// <summary>
  /// Starts an asynchronous branch chain with the first condition and executable.
  /// </summary>
  /// <param name="condition">Condition for selecting the first branch.</param>
  /// <param name="executable">Asynchronous executable invoked when the condition is satisfied.</param>
  /// <returns>Asynchronous branch builder initialized with the first node.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  public static AsyncBranchBuilder<T1, T2> If(Func<T1, bool> condition, IAsyncExecutable<T1, T2> executable) {
    return AsyncBranchBuilder<T1, T2>.If(condition, executable);
  }

  /// <summary>
  /// Starts an asynchronous branch chain with the first condition and delegate-based executable.
  /// </summary>
  /// <param name="condition">Condition for selecting the first branch.</param>
  /// <param name="func">Asynchronous delegate invoked when the condition is satisfied.</param>
  /// <returns>Asynchronous branch builder initialized with the first node.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="func"/> is <see langword="null"/>.
  /// </exception>
  public static AsyncBranchBuilder<T1, T2> If(Func<T1, bool> condition, AsyncFunc<T1, T2> func) {
    return If(condition, AsyncExecutable.Create(func));
  }

}

/// <summary>
/// Factory methods for creating asynchronous conditional branch builders for single-parameter and parameterless executables.
/// </summary>
public static class AsyncBranch<T> {

  /// <summary>
  /// Starts an asynchronous branch chain with the first condition and executable.
  /// </summary>
  /// <param name="condition">Condition for selecting the first branch.</param>
  /// <param name="executable">Asynchronous executable invoked when the condition is satisfied.</param>
  /// <returns>Asynchronous branch builder initialized with the first node.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  public static AsyncBranchBuilder<T, Unit> If(Func<T, bool> condition, IAsyncExecutable<T, Unit> executable) {
    return AsyncBranchBuilder<T, Unit>.If(condition, executable);
  }

  /// <summary>
  /// Starts an asynchronous branch chain with the first condition and action-based executable.
  /// </summary>
  /// <param name="condition">Condition for selecting the first branch.</param>
  /// <param name="action">Asynchronous action invoked when the condition is satisfied.</param>
  /// <returns>Asynchronous branch builder initialized with the first node.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="action"/> is <see langword="null"/>.
  /// </exception>
  public static AsyncBranchBuilder<T, Unit> If(Func<T, bool> condition, AsyncAction<T> action) {
    return If(condition, AsyncExecutable.Create(action));
  }

  /// <summary>
  /// Starts a parameterless asynchronous branch chain with the first condition and executable.
  /// </summary>
  /// <param name="condition">Condition for selecting the first branch.</param>
  /// <param name="executable">Asynchronous executable invoked when the condition is satisfied.</param>
  /// <returns>Asynchronous branch builder initialized with the first node.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  public static AsyncBranchBuilder<Unit, T> If(Func<bool> condition, IAsyncExecutable<Unit, T> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return AsyncBranchBuilder<Unit, T>.If(_ => condition(), executable);
  }

  /// <summary>
  /// Starts a parameterless asynchronous branch chain with the first condition and delegate-based executable.
  /// </summary>
  /// <param name="condition">Condition for selecting the first branch.</param>
  /// <param name="action">Asynchronous delegate invoked when the condition is satisfied.</param>
  /// <returns>Asynchronous branch builder initialized with the first node.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="action"/> is <see langword="null"/>.
  /// </exception>
  public static AsyncBranchBuilder<Unit, T> If(Func<bool> condition, AsyncFunc<T> action) {
    return If(condition, AsyncExecutable.Create(action));
  }

}

/// <summary>
/// Factory methods for creating parameterless asynchronous conditional branch builders with no result.
/// </summary>
public static class AsyncBranch {

  /// <summary>
  /// Starts a parameterless asynchronous branch chain with the first condition and executable.
  /// </summary>
  /// <param name="condition">Condition for selecting the first branch.</param>
  /// <param name="executable">Asynchronous executable invoked when the condition is satisfied.</param>
  /// <returns>Asynchronous branch builder initialized with the first node.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  public static AsyncBranchBuilder<Unit, Unit> If(Func<bool> condition, IAsyncExecutable<Unit, Unit> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return AsyncBranchBuilder<Unit, Unit>.If(_ => condition(), executable);
  }

  /// <summary>
  /// Starts a parameterless asynchronous branch chain with the first condition and action-based executable.
  /// </summary>
  /// <param name="condition">Condition for selecting the first branch.</param>
  /// <param name="action">Asynchronous action invoked when the condition is satisfied.</param>
  /// <returns>Asynchronous branch builder initialized with the first node.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="action"/> is <see langword="null"/>.
  /// </exception>
  public static AsyncBranchBuilder<Unit, Unit> If(Func<bool> condition, AsyncAction action) {
    return If(condition, AsyncExecutable.Create(action));
  }

}