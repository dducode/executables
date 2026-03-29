using Interactions.Internal;

namespace Interactions.Branches;

/// <summary>
/// Factory methods for creating conditional branch builders with input and output values.
/// </summary>
public static class Branch<T1, T2> {

  /// <summary>
  /// Starts a branch chain with the first condition and executable.
  /// </summary>
  /// <param name="condition">Condition for selecting the first branch.</param>
  /// <param name="executable">Executable invoked when the condition is satisfied.</param>
  /// <returns>Branch builder initialized with the first node.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  public static BranchBuilder<T1, T2> If(Func<T1, bool> condition, IExecutable<T1, T2> executable) {
    return BranchBuilder<T1, T2>.If(condition, executable);
  }

  /// <summary>
  /// Starts a branch chain with the first condition and delegate-based executable.
  /// </summary>
  /// <param name="condition">Condition for selecting the first branch.</param>
  /// <param name="func">Delegate invoked when the condition is satisfied.</param>
  /// <returns>Branch builder initialized with the first node.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="func"/> is <see langword="null"/>.
  /// </exception>
  public static BranchBuilder<T1, T2> If(Func<T1, bool> condition, Func<T1, T2> func) {
    return If(condition, Executable.Create(func));
  }

}

/// <summary>
/// Factory methods for creating conditional branch builders for single-parameter and parameterless executables.
/// </summary>
public static class Branch<T> {

  /// <summary>
  /// Starts a branch chain with the first condition and executable.
  /// </summary>
  /// <param name="condition">Condition for selecting the first branch.</param>
  /// <param name="executable">Executable invoked when the condition is satisfied.</param>
  /// <returns>Branch builder initialized with the first node.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  public static BranchBuilder<T, Unit> If(Func<T, bool> condition, IExecutable<T, Unit> executable) {
    return BranchBuilder<T, Unit>.If(condition, executable);
  }

  /// <summary>
  /// Starts a branch chain with the first condition and action-based executable.
  /// </summary>
  /// <param name="condition">Condition for selecting the first branch.</param>
  /// <param name="action">Action invoked when the condition is satisfied.</param>
  /// <returns>Branch builder initialized with the first node.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="action"/> is <see langword="null"/>.
  /// </exception>
  public static BranchBuilder<T, Unit> If(Func<T, bool> condition, Action<T> action) {
    return If(condition, Executable.Create(action));
  }

  /// <summary>
  /// Starts a parameterless branch chain with the first condition and executable.
  /// </summary>
  /// <param name="condition">Condition for selecting the first branch.</param>
  /// <param name="executable">Executable invoked when the condition is satisfied.</param>
  /// <returns>Branch builder initialized with the first node.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  public static BranchBuilder<Unit, T> If(Func<bool> condition, IExecutable<Unit, T> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return BranchBuilder<Unit, T>.If(_ => condition(), executable);
  }

  /// <summary>
  /// Starts a parameterless branch chain with the first condition and delegate-based executable.
  /// </summary>
  /// <param name="condition">Condition for selecting the first branch.</param>
  /// <param name="action">Delegate invoked when the condition is satisfied.</param>
  /// <returns>Branch builder initialized with the first node.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="action"/> is <see langword="null"/>.
  /// </exception>
  public static BranchBuilder<Unit, T> If(Func<bool> condition, Func<T> action) {
    return If(condition, Executable.Create(action));
  }

}

/// <summary>
/// Factory methods for creating parameterless conditional branch builders with no result.
/// </summary>
public static class Branch {

  /// <summary>
  /// Starts a parameterless branch chain with the first condition and executable.
  /// </summary>
  /// <param name="condition">Condition for selecting the first branch.</param>
  /// <param name="executable">Executable invoked when the condition is satisfied.</param>
  /// <returns>Branch builder initialized with the first node.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="executable"/> is <see langword="null"/>.
  /// </exception>
  public static BranchBuilder<Unit, Unit> If(Func<bool> condition, IExecutable<Unit, Unit> executable) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return BranchBuilder<Unit, Unit>.If(_ => condition(), executable);
  }

  /// <summary>
  /// Starts a parameterless branch chain with the first condition and action-based executable.
  /// </summary>
  /// <param name="condition">Condition for selecting the first branch.</param>
  /// <param name="action">Action invoked when the condition is satisfied.</param>
  /// <returns>Branch builder initialized with the first node.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="action"/> is <see langword="null"/>.
  /// </exception>
  public static BranchBuilder<Unit, Unit> If(Func<bool> condition, Action action) {
    return If(condition, Executable.Create(action));
  }

}