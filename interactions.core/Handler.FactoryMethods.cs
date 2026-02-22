using System.Diagnostics.Contracts;
using Interactions.Core.Handlers;

namespace Interactions.Core;

public static class Handler {

  [Pure]
  public static Handler<T, T> Identity<T>() {
    return IdentityHandler<T>.Instance;
  }

  [Pure]
  public static Handler<T1, T2> FromMethod<T1, T2>(Func<T1, T2> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new AnonymousHandler_Func<T1, T2>(func);
  }

  [Pure]
  public static Handler<T, T> FromMethod<T>(Func<T, T> func) {
    return FromMethod<T, T>(func);
  }

  [Pure]
  public static Handler<Unit, T> FromMethod<T>(Func<T> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new AnonymousHandler_Func<T>(action);
  }

  [Pure]
  public static Handler<T, Unit> FromMethod<T>(Action<T> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new AnonymousHandler_Action<T>(action);
  }

  [Pure]
  public static Handler<Unit, Unit> FromMethod(Action action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new AnonymousHandler_Action(action);
  }

  [Pure]
  public static AsyncHandler<T1, T2> FromMethod<T1, T2>(AsyncFunc<T1, T2> func) {
    ExceptionsHelper.ThrowIfNull(func, nameof(func));
    return new AsyncAnonymousHandler_Func<T1, T2>(func);
  }

  [Pure]
  public static AsyncHandler<T, T> FromMethod<T>(AsyncFunc<T, T> func) {
    return FromMethod<T, T>(func);
  }

  [Pure]
  public static AsyncHandler<Unit, T> FromMethod<T>(AsyncFunc<T> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new AsyncAnonymousHandler_Func<T>(action);
  }

  [Pure]
  public static AsyncHandler<T, Unit> FromMethod<T>(AsyncAction<T> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new AsyncAnonymousHandler_Action<T>(action);
  }

  [Pure]
  public static AsyncHandler<Unit, Unit> FromMethod(AsyncAction action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new AsyncAnonymousHandler_Action(action);
  }

}