using System.Diagnostics.Contracts;
using Executables.Internal;

namespace Executables.Branches;

public static class BranchBuilderExtensions {

  /// <summary>
  /// Adds a conditional branch backed by a delegate.
  /// </summary>
  /// <param name="builder">Target branch builder.</param>
  /// <param name="condition">Condition for selecting this branch.</param>
  /// <param name="action">Delegate invoked when the condition is satisfied.</param>
  /// <returns>Updated branch builder.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="action"/> is <see langword="null"/>.
  /// </exception>
  public static BranchBuilder<T1, T2> ElseIf<T1, T2>(this BranchBuilder<T1, T2> builder, Func<T1, bool> condition, Func<T1, T2> action) {
    return builder.ElseIf(condition, Executable.Create(action));
  }

  /// <summary>
  /// Adds a parameterless conditional branch backed by a delegate.
  /// </summary>
  /// <param name="builder">Target branch builder.</param>
  /// <param name="condition">Condition for selecting this branch.</param>
  /// <param name="action">Delegate invoked when the condition is satisfied.</param>
  /// <returns>Updated branch builder.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="action"/> is <see langword="null"/>.
  /// </exception>
  public static BranchBuilder<Unit, T> ElseIf<T>(this BranchBuilder<Unit, T> builder, Func<bool> condition, Func<T> action) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return builder.ElseIf(_ => condition(), Executable.Create(action));
  }

  /// <summary>
  /// Adds a conditional branch backed by an action.
  /// </summary>
  /// <param name="builder">Target branch builder.</param>
  /// <param name="condition">Condition for selecting this branch.</param>
  /// <param name="action">Action invoked when the condition is satisfied.</param>
  /// <returns>Updated branch builder.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="action"/> is <see langword="null"/>.
  /// </exception>
  public static BranchBuilder<T, Unit> ElseIf<T>(this BranchBuilder<T, Unit> builder, Func<T, bool> condition, Action<T> action) {
    return builder.ElseIf(condition, Executable.Create(action));
  }

  /// <summary>
  /// Adds a parameterless conditional branch backed by an action.
  /// </summary>
  /// <param name="builder">Target branch builder.</param>
  /// <param name="condition">Condition for selecting this branch.</param>
  /// <param name="action">Action invoked when the condition is satisfied.</param>
  /// <returns>Updated branch builder.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="action"/> is <see langword="null"/>.
  /// </exception>
  public static BranchBuilder<Unit, Unit> ElseIf(this BranchBuilder<Unit, Unit> builder, Func<bool> condition, Action action) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return builder.ElseIf(_ => condition(), Executable.Create(action));
  }

  /// <summary>
  /// Finalizes the branch chain with a delegate-based fallback.
  /// </summary>
  /// <param name="builder">Target branch builder.</param>
  /// <param name="action">Fallback delegate executed when no condition matches.</param>
  /// <returns>Composed executable with all branch conditions.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T2> Else<T1, T2>(this BranchBuilder<T1, T2> builder, Func<T1, T2> action) {
    return builder.Else(Executable.Create(action));
  }

  /// <summary>
  /// Finalizes a parameterless branch chain with a delegate-based fallback.
  /// </summary>
  /// <param name="builder">Target branch builder.</param>
  /// <param name="action">Fallback delegate executed when no condition matches.</param>
  /// <returns>Composed executable with all branch conditions.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<Unit, T> Else<T>(this BranchBuilder<Unit, T> builder, Func<T> action) {
    return builder.Else(Executable.Create(action));
  }

  /// <summary>
  /// Finalizes the branch chain with an action-based fallback.
  /// </summary>
  /// <param name="builder">Target branch builder.</param>
  /// <param name="action">Fallback action executed when no condition matches.</param>
  /// <returns>Composed executable with all branch conditions.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T, Unit> Else<T>(this BranchBuilder<T, Unit> builder, Action<T> action) {
    return builder.Else(Executable.Create(action));
  }

  /// <summary>
  /// Finalizes a parameterless branch chain with an action-based fallback.
  /// </summary>
  /// <param name="builder">Target branch builder.</param>
  /// <param name="action">Fallback action executed when no condition matches.</param>
  /// <returns>Composed executable with all branch conditions.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<Unit, Unit> Else(this BranchBuilder<Unit, Unit> builder, Action action) {
    return builder.Else(Executable.Create(action));
  }

}