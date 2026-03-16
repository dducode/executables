using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.Branches;

public static class AsyncBranch<T1, T2> {

  public static AsyncBranchBuilder<T1, T2> If(Func<T1, bool> condition, IAsyncExecutable<T1, T2> handler) {
    return AsyncBranchBuilder<T1, T2>.If(condition, handler);
  }

  public static AsyncBranchBuilder<T1, T2> If(Func<T1, bool> condition, AsyncFunc<T1, T2> func) {
    return If(condition, AsyncExecutable.Create(func));
  }

}

public static class AsyncBranch<T> {

  public static AsyncBranchBuilder<T, Unit> If(Func<T, bool> condition, IAsyncExecutable<T, Unit> handler) {
    return AsyncBranchBuilder<T, Unit>.If(condition, handler);
  }

  public static AsyncBranchBuilder<T, Unit> If(Func<T, bool> condition, AsyncAction<T> action) {
    return If(condition, AsyncExecutable.Create(action));
  }

  public static AsyncBranchBuilder<Unit, T> If(Func<bool> condition, IAsyncExecutable<Unit, T> handler) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return AsyncBranchBuilder<Unit, T>.If(_ => condition(), handler);
  }

  public static AsyncBranchBuilder<Unit, T> If(Func<bool> condition, AsyncFunc<T> action) {
    return If(condition, AsyncExecutable.Create(action));
  }

}

public static class AsyncBranch {

  public static AsyncBranchBuilder<Unit, Unit> If(Func<bool> condition, IAsyncExecutable<Unit, Unit> handler) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    return AsyncBranchBuilder<Unit, Unit>.If(_ => condition(), handler);
  }

  public static AsyncBranchBuilder<Unit, Unit> If(Func<bool> condition, AsyncAction action) {
    return If(condition, AsyncExecutable.Create(action));
  }

}