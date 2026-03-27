using System.Diagnostics.Contracts;
using Interactions.Core.Handleables;
using Interactions.Internal;

namespace Interactions.Handling;

public static partial class HandleableExtensions {

  [Pure]
  public static IHandleable<T1, T2, THandler> Merge<T1, T2, THandler>(
    this IHandleable<T1, T2, THandler> first,
    IHandleable<T1, T2, THandler> second) where THandler : Handler<T1, T2> {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositeHandleable<T1, T2, THandler>(first, second);
  }

  public static IDisposable Handle<T1, T2>(this IHandleable<T1, T2> handleable, Func<T1, T2> handler) {
    return handleable.Handle(Executable.Create(handler).AsHandler());
  }

  public static IDisposable Handle<T>(this IHandleable<Unit, T> handleable, Func<T> handler) {
    return handleable.Handle(Executable.Create(handler).AsHandler());
  }

  public static IDisposable Handle<T>(this IHandleable<T, Unit> handleable, Action<T> handler) {
    return handleable.Handle(Executable.Create(handler).AsHandler());
  }

  public static IDisposable Handle(this IHandleable<Unit, Unit> handleable, Action handler) {
    return handleable.Handle(Executable.Create(handler).AsHandler());
  }

}