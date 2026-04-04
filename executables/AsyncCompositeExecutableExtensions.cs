using System.Diagnostics.Contracts;
using Executables.Core.Executables;
using Executables.Internal;

namespace Executables;

public static class AsyncCompositeExecutableExtensions {

  /// <summary>
  /// Prepends an asynchronous executable to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Executable invoked first.</param>
  /// <returns>Asynchronous executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Compose<T1, T2, T3>(this IAsyncExecutable<T2, T3> first, IAsyncExecutable<T1, T2> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new AsyncCompositeExecutable<T1, T2, T3>(first, second);
  }

  /// <summary>
  /// Prepends an asynchronous delegate to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Asynchronous delegate invoked first.</param>
  /// <returns>Asynchronous executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Compose<T1, T2, T3>(this IAsyncExecutable<T2, T3> first, AsyncFunc<T1, T2> second) {
    return first.Compose(AsyncExecutable.Create(second));
  }

  /// <summary>
  /// Prepends a parameterless asynchronous delegate to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Parameterless asynchronous delegate invoked first.</param>
  /// <returns>Asynchronous executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<Unit, T2> Compose<T1, T2>(this IAsyncExecutable<T1, T2> first, AsyncFunc<T1> second) {
    return first.Compose(AsyncExecutable.Create(second));
  }

  /// <summary>
  /// Prepends an asynchronous action to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Asynchronous action invoked first.</param>
  /// <returns>Asynchronous executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> Compose<T1, T2>(this IAsyncExecutable<Unit, T2> first, AsyncAction<T1> second) {
    return first.Compose(AsyncExecutable.Create(second));
  }

  /// <summary>
  /// Prepends a parameterless asynchronous action to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Parameterless asynchronous action invoked first.</param>
  /// <returns>Asynchronous executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<Unit, T> Compose<T>(this IAsyncExecutable<Unit, T> first, AsyncAction second) {
    return first.Compose(AsyncExecutable.Create(second));
  }

  /// <summary>
  /// Prepends a synchronous executable to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Asynchronous executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Synchronous executable invoked first.</param>
  /// <returns>Asynchronous executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Compose<T1, T2, T3>(this IAsyncExecutable<T2, T3> first, IExecutable<T1, T2> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return first.Compose(second.ToAsyncExecutable());
  }

  /// <summary>
  /// Prepends a synchronous delegate to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Asynchronous executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Delegate invoked first.</param>
  /// <returns>Asynchronous executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Compose<T1, T2, T3>(this IAsyncExecutable<T2, T3> first, Func<T1, T2> second) {
    return first.Compose(Executable.Create(second));
  }

  /// <summary>
  /// Prepends a parameterless synchronous delegate to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Asynchronous executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Parameterless delegate invoked first.</param>
  /// <returns>Asynchronous executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<Unit, T2> Compose<T1, T2>(this IAsyncExecutable<T1, T2> first, Func<T1> second) {
    return first.Compose(Executable.Create(second));
  }

  /// <summary>
  /// Prepends a synchronous action to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Asynchronous executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Action invoked first.</param>
  /// <returns>Asynchronous executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> Compose<T1, T2>(this IAsyncExecutable<Unit, T2> first, Action<T1> second) {
    return first.Compose(Executable.Create(second));
  }

  /// <summary>
  /// Prepends a parameterless synchronous action to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Asynchronous executable invoked after <paramref name="second"/>.</param>
  /// <param name="second">Parameterless action invoked first.</param>
  /// <returns>Asynchronous executable that runs <paramref name="second"/> and then <paramref name="first"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<Unit, T> Compose<T>(this IAsyncExecutable<Unit, T> first, Action second) {
    return first.Compose(Executable.Create(second));
  }

  /// <summary>
  /// Appends an asynchronous executable to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Executable invoked with the result of <paramref name="first"/>.</param>
  /// <returns>Asynchronous executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Then<T1, T2, T3>(this IAsyncExecutable<T1, T2> first, IAsyncExecutable<T2, T3> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return second.Compose(first);
  }

  /// <summary>
  /// Appends an asynchronous delegate to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Asynchronous delegate invoked with the result of <paramref name="first"/>.</param>
  /// <returns>Asynchronous executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Then<T1, T2, T3>(this IAsyncExecutable<T1, T2> first, AsyncFunc<T2, T3> second) {
    return first.Then(AsyncExecutable.Create(second));
  }

  /// <summary>
  /// Appends a parameterless asynchronous delegate to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Parameterless asynchronous delegate invoked after <paramref name="first"/> completes.</param>
  /// <returns>Asynchronous executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> Then<T1, T2>(this IAsyncExecutable<T1, Unit> first, AsyncFunc<T2> second) {
    return first.Then(AsyncExecutable.Create(second));
  }

  /// <summary>
  /// Appends an asynchronous action to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Asynchronous action invoked with the result of <paramref name="first"/>.</param>
  /// <returns>Asynchronous executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, Unit> Then<T1, T2>(this IAsyncExecutable<T1, T2> first, AsyncAction<T2> second) {
    return first.Then(AsyncExecutable.Create(second));
  }

  /// <summary>
  /// Appends a parameterless asynchronous action to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Parameterless asynchronous action invoked after <paramref name="first"/> completes.</param>
  /// <returns>Asynchronous executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T, Unit> Then<T>(this IAsyncExecutable<T, Unit> first, AsyncAction second) {
    return first.Then(AsyncExecutable.Create(second));
  }

  /// <summary>
  /// Appends a synchronous executable to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Asynchronous executable invoked first.</param>
  /// <param name="second">Synchronous executable invoked with the result of <paramref name="first"/>.</param>
  /// <returns>Asynchronous executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Then<T1, T2, T3>(this IAsyncExecutable<T1, T2> first, IExecutable<T2, T3> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return first.Then(second.ToAsyncExecutable());
  }

  /// <summary>
  /// Appends a synchronous delegate to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Delegate invoked with the result of <paramref name="first"/>.</param>
  /// <returns>Asynchronous executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Then<T1, T2, T3>(this IAsyncExecutable<T1, T2> first, Func<T2, T3> second) {
    return first.Then(Executable.Create(second));
  }

  /// <summary>
  /// Appends a parameterless synchronous delegate to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Parameterless delegate invoked after <paramref name="first"/> completes.</param>
  /// <returns>Asynchronous executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> Then<T1, T2>(this IAsyncExecutable<T1, Unit> first, Func<T2> second) {
    return first.Then(Executable.Create(second));
  }

  /// <summary>
  /// Appends a synchronous action to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Action invoked with the result of <paramref name="first"/>.</param>
  /// <returns>Asynchronous executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, Unit> Then<T1, T2>(this IAsyncExecutable<T1, T2> first, Action<T2> second) {
    return first.Then(Executable.Create(second));
  }

  /// <summary>
  /// Appends a parameterless synchronous action to the current asynchronous executable, creating a single asynchronous pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Parameterless action invoked after <paramref name="first"/> completes.</param>
  /// <returns>Asynchronous executable that runs <paramref name="first"/> and then <paramref name="second"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T, Unit> Then<T>(this IAsyncExecutable<T, Unit> first, Action second) {
    return first.Then(Executable.Create(second));
  }

}