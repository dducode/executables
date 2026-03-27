using Interactions.Internal;

namespace Interactions.Branches;

public static class Branch<T1, T2> {

  public static BranchBuilder<T1, T2> If(Func<T1, bool> condition, IExecutable<T1, T2> handler) {
    return BranchBuilder<T1, T2>.If(condition, handler);
  }

  public static BranchBuilder<T1, T2> If(Func<T1, bool> condition, Func<T1, T2> func) {
    return If(condition, Executable.Create(func));
  }

}

public static class Branch<T> {

  public static BranchBuilder<T, Unit> If(Func<T, bool> condition, IExecutable<T, Unit> handler) {
    return BranchBuilder<T, Unit>.If(condition, handler);
  }

  public static BranchBuilder<T, Unit> If(Func<T, bool> condition, Action<T> action) {
    return If(condition, Executable.Create(action));
  }

  public static BranchBuilder<Unit, T> If(Func<bool> condition, IExecutable<Unit, T> handler) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return BranchBuilder<Unit, T>.If(_ => condition(), handler);
  }

  public static BranchBuilder<Unit, T> If(Func<bool> condition, Func<T> action) {
    return If(condition, Executable.Create(action));
  }

}

public static class Branch {

  public static BranchBuilder<Unit, Unit> If(Func<bool> condition, IExecutable<Unit, Unit> handler) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return BranchBuilder<Unit, Unit>.If(_ => condition(), handler);
  }

  public static BranchBuilder<Unit, Unit> If(Func<bool> condition, Action action) {
    return If(condition, Executable.Create(action));
  }

}