using System.Diagnostics.Contracts;
using Executables.Core.Handleables;
using Executables.Internal;

namespace Executables.Handling;

/// <summary>
/// Extension methods for handleable objects.
/// </summary>
public static class HandleableExtensions {

  /// <summary>
  /// Merges two handleables into one registration target.
  /// </summary>
  /// <param name="first">First handleable.</param>
  /// <param name="second">Second handleable.</param>
  /// <returns>Composite handleable that registers handlers in both sources.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IHandleable<T1, T2, THandler> Merge<T1, T2, THandler>(
    this IHandleable<T1, T2, THandler> first,
    IHandleable<T1, T2, THandler> second) where THandler : Handler<T1, T2> {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositeHandleable<T1, T2, THandler>(first, second);
  }

  /// <summary>
  /// Registers a function as a handler.
  /// </summary>
  public static IDisposable Handle<T1, T2>(this IHandleable<T1, T2> handleable, Func<T1, T2> handler) {
    return handleable.Handle(Executable.Create(handler).AsHandler());
  }

  /// <summary>
  /// Registers a parameterless function as a handler.
  /// </summary>
  public static IDisposable Handle<T>(this IHandleable<Unit, T> handleable, Func<T> handler) {
    return handleable.Handle(Executable.Create(handler).AsHandler());
  }

  /// <summary>
  /// Registers an action as a handler.
  /// </summary>
  public static IDisposable Handle<T>(this IHandleable<T, Unit> handleable, Action<T> handler) {
    return handleable.Handle(Executable.Create(handler).AsHandler());
  }

  /// <summary>
  /// Registers a parameterless action as a handler.
  /// </summary>
  public static IDisposable Handle(this IHandleable<Unit, Unit> handleable, Action handler) {
    return handleable.Handle(Executable.Create(handler).AsHandler());
  }

}