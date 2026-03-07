using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Core.Handleables;

public static partial class HandleableExtensions {

  [Pure]
  public static Handleable<T1, T2> Merge<T1, T2>(this Handleable<T1, T2> first, Handleable<T1, T2> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new MergedHandleable<T1, T2>(first, second);
  }

  public static IDisposable Handle<T1, T2>(this Handleable<T1, T2> handleable, Func<T1, T2> handler) {
    return handleable.Handle(Handler.Create(handler));
  }

  public static IDisposable Handle<T>(this Handleable<Unit, T> handleable, Func<T> handler) {
    return handleable.Handle(Handler.Create(handler));
  }

  public static IDisposable Handle<T>(this Handleable<T, Unit> handleable, Action<T> handler) {
    return handleable.Handle(Handler.Create(handler));
  }

  public static IDisposable Handle(this Handleable<Unit, Unit> handleable, Action handler) {
    return handleable.Handle(Handler.Create(handler));
  }

}