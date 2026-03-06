namespace Interactions.Core;

public static class Branch<T1, T2> {

  public static BranchBuilder<T1, T2> If(Func<bool> condition, Handler<T1, T2> handler) {
    return BranchBuilder<T1, T2>.If(condition, handler);
  }

  public static BranchBuilder<T1, T2> If(Func<bool> condition, Func<T1, T2> func) {
    return If(condition, Handler.FromMethod(func));
  }

  public static AsyncBranchBuilder<T1, T2> If(Func<bool> condition, AsyncHandler<T1, T2> handler) {
    return AsyncBranchBuilder<T1, T2>.If(condition, handler);
  }

  public static AsyncBranchBuilder<T1, T2> If(Func<bool> condition, AsyncFunc<T1, T2> func) {
    return If(condition, AsyncHandler.FromMethod(func));
  }

}

public static class Branch<T> {

  public static BranchBuilder<T, Unit> If(Func<bool> condition, Handler<T, Unit> handler) {
    return BranchBuilder<T, Unit>.If(condition, handler);
  }

  public static BranchBuilder<T, Unit> If(Func<bool> condition, Action<T> action) {
    return If(condition, Handler.FromMethod(action));
  }

  public static BranchBuilder<Unit, T> If(Func<bool> condition, Handler<Unit, T> handler) {
    return BranchBuilder<Unit, T>.If(condition, handler);
  }

  public static BranchBuilder<Unit, T> If(Func<bool> condition, Func<T> action) {
    return If(condition, Handler.FromMethod(action));
  }

  public static AsyncBranchBuilder<T, Unit> If(Func<bool> condition, AsyncHandler<T, Unit> handler) {
    return AsyncBranchBuilder<T, Unit>.If(condition, handler);
  }

  public static AsyncBranchBuilder<T, Unit> If(Func<bool> condition, AsyncAction<T> action) {
    return If(condition, AsyncHandler.FromMethod(action));
  }

  public static AsyncBranchBuilder<Unit, T> If(Func<bool> condition, AsyncHandler<Unit, T> handler) {
    return AsyncBranchBuilder<Unit, T>.If(condition, handler);
  }

  public static AsyncBranchBuilder<Unit, T> If(Func<bool> condition, AsyncFunc<T> action) {
    return If(condition, AsyncHandler.FromMethod(action));
  }

}

public static class Branch {

  public static BranchBuilder<Unit, Unit> If(Func<bool> condition, Handler<Unit, Unit> handler) {
    return BranchBuilder<Unit, Unit>.If(condition, handler);
  }

  public static BranchBuilder<Unit, Unit> If(Func<bool> condition, Action action) {
    return If(condition, Handler.FromMethod(action));
  }

  public static AsyncBranchBuilder<Unit, Unit> If(Func<bool> condition, AsyncHandler<Unit, Unit> handler) {
    return AsyncBranchBuilder<Unit, Unit>.If(condition, handler);
  }

  public static AsyncBranchBuilder<Unit, Unit> If(Func<bool> condition, AsyncAction action) {
    return If(condition, AsyncHandler.FromMethod(action));
  }

}