using System.Diagnostics.Contracts;
using Executables.Internal;

namespace Executables.Branches;

public static class AsyncBranchBuilderExtensions {

  /// <summary>
  /// Adds an asynchronous conditional branch backed by a delegate.
  /// </summary>
  /// <param name="builder">Target asynchronous branch builder.</param>
  /// <param name="condition">Condition for selecting this branch.</param>
  /// <param name="action">Asynchronous delegate invoked when the condition is satisfied.</param>
  /// <returns>Updated asynchronous branch builder.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="action"/> is <see langword="null"/>.
  /// </exception>
  public static AsyncBranchBuilder<T1, T2> ElseIf<T1, T2>(this AsyncBranchBuilder<T1, T2> builder, Func<T1, bool> condition, AsyncFunc<T1, T2> action) {
    return builder.ElseIf(condition, AsyncExecutable.Create(action));
  }

  /// <summary>
  /// Adds a parameterless asynchronous conditional branch backed by a delegate.
  /// </summary>
  /// <param name="builder">Target asynchronous branch builder.</param>
  /// <param name="condition">Condition for selecting this branch.</param>
  /// <param name="action">Asynchronous delegate invoked when the condition is satisfied.</param>
  /// <returns>Updated asynchronous branch builder.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="action"/> is <see langword="null"/>.
  /// </exception>
  public static AsyncBranchBuilder<Unit, T> ElseIf<T>(this AsyncBranchBuilder<Unit, T> builder, Func<bool> condition, AsyncFunc<T> action) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return builder.ElseIf(_ => condition(), AsyncExecutable.Create(action));
  }

  /// <summary>
  /// Adds an asynchronous conditional branch backed by an action.
  /// </summary>
  /// <param name="builder">Target asynchronous branch builder.</param>
  /// <param name="condition">Condition for selecting this branch.</param>
  /// <param name="action">Asynchronous action invoked when the condition is satisfied.</param>
  /// <returns>Updated asynchronous branch builder.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="action"/> is <see langword="null"/>.
  /// </exception>
  public static AsyncBranchBuilder<T, Unit> ElseIf<T>(this AsyncBranchBuilder<T, Unit> builder, Func<T, bool> condition, AsyncAction<T> action) {
    return builder.ElseIf(condition, AsyncExecutable.Create(action));
  }

  /// <summary>
  /// Adds a parameterless asynchronous conditional branch backed by an action.
  /// </summary>
  /// <param name="builder">Target asynchronous branch builder.</param>
  /// <param name="condition">Condition for selecting this branch.</param>
  /// <param name="action">Asynchronous action invoked when the condition is satisfied.</param>
  /// <returns>Updated asynchronous branch builder.</returns>
  /// <exception cref="ArgumentNullException">
  /// <paramref name="condition"/> or <paramref name="action"/> is <see langword="null"/>.
  /// </exception>
  public static AsyncBranchBuilder<Unit, Unit> ElseIf(this AsyncBranchBuilder<Unit, Unit> builder, Func<bool> condition, AsyncAction action) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return builder.ElseIf(_ => condition(), AsyncExecutable.Create(action));
  }

  /// <summary>
  /// Finalizes the asynchronous branch chain with a delegate-based fallback.
  /// </summary>
  /// <param name="builder">Target asynchronous branch builder.</param>
  /// <param name="action">Fallback asynchronous delegate executed when no condition matches.</param>
  /// <returns>Composed asynchronous executable with all branch conditions.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> Else<T1, T2>(this AsyncBranchBuilder<T1, T2> builder, AsyncFunc<T1, T2> action) {
    return builder.Else(AsyncExecutable.Create(action));
  }

  /// <summary>
  /// Finalizes a parameterless asynchronous branch chain with a delegate-based fallback.
  /// </summary>
  /// <param name="builder">Target asynchronous branch builder.</param>
  /// <param name="action">Fallback asynchronous delegate executed when no condition matches.</param>
  /// <returns>Composed asynchronous executable with all branch conditions.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<Unit, T> Else<T>(this AsyncBranchBuilder<Unit, T> builder, AsyncFunc<T> action) {
    return builder.Else(AsyncExecutable.Create(action));
  }

  /// <summary>
  /// Finalizes the asynchronous branch chain with an action-based fallback.
  /// </summary>
  /// <param name="builder">Target asynchronous branch builder.</param>
  /// <param name="action">Fallback asynchronous action executed when no condition matches.</param>
  /// <returns>Composed asynchronous executable with all branch conditions.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T, Unit> Else<T>(this AsyncBranchBuilder<T, Unit> builder, AsyncAction<T> action) {
    return builder.Else(AsyncExecutable.Create(action));
  }

  /// <summary>
  /// Finalizes a parameterless asynchronous branch chain with an action-based fallback.
  /// </summary>
  /// <param name="builder">Target asynchronous branch builder.</param>
  /// <param name="action">Fallback asynchronous action executed when no condition matches.</param>
  /// <returns>Composed asynchronous executable with all branch conditions.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<Unit, Unit> Else(this AsyncBranchBuilder<Unit, Unit> builder, AsyncAction action) {
    return builder.Else(AsyncExecutable.Create(action));
  }

}