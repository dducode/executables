using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Core.Internal;
using Interactions.Operations;

namespace Interactions.Executables;

public static partial class ExecutableExtensions {

  [Pure]
  public static IExecutable<T1, T3> Then<T1, T2, T3>(this IExecutable<T1, T2> first, IExecutable<T2, T3> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    if (first is IdentityExecutable<T1>)
      return (IExecutable<T1, T3>)second;
    return new CompositeExecutable<T1, T2, T3>(first, second);
  }

  [Pure]
  public static IExecutable<T1, T3> Then<T1, T2, T3>(this IExecutable<T1, T2> executable, Func<T2, T3> next) {
    return executable.Then(Executable.Create(next));
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> Then<T1, T2, T3>(this IExecutable<T1, T2> first, IAsyncExecutable<T2, T3> second) {
    return first.ToAsyncExecutable().Then(second);
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> Then<T1, T2, T3>(this IExecutable<T1, T2> executable, AsyncFunc<T2, T3> next) {
    return executable.ToAsyncExecutable().Then(AsyncExecutable.Create(next));
  }

  [Pure]
  public static IExecutable<T1, (T3, T4)> Fork<T1, T2, T3, T4>(
    this IExecutable<T1, T2> executable,
    IExecutable<T2, T3> firstBranch,
    IExecutable<T2, T4> secondBranch) {
    ExceptionsHelper.ThrowIfNull(firstBranch, nameof(firstBranch));
    ExceptionsHelper.ThrowIfNull(secondBranch, nameof(secondBranch));
    return executable.Then(new ForkExecutable<T2, T3, T4>(firstBranch, secondBranch));
  }

  [Pure]
  public static IExecutable<T1, (T3, T4)> Fork<T1, T2, T3, T4>(this IExecutable<T1, T2> executable, Func<T2, T3> firstBranch, Func<T2, T4> secondBranch) {
    return executable.Fork(Executable.Create(firstBranch), Executable.Create(secondBranch));
  }

  [Pure]
  public static IExecutable<T1, (T3, T2)> Swap<T1, T2, T3>(this IExecutable<T1, (T2, T3)> fork) {
    return fork.Then(x => (x.Item2, x.Item1));
  }

  [Pure]
  public static IExecutable<T1, (TNew, T3)> First<T1, T2, T3, TNew>(this IExecutable<T1, (T2, T3)> fork, IExecutable<T2, TNew> map) {
    ExceptionsHelper.ThrowIfNull(map, nameof(map));
    return fork.Then(new FirstMapExecutable<T2, T3, TNew>(map));
  }

  [Pure]
  public static IExecutable<T1, (TNew, T3)> First<T1, T2, T3, TNew>(this IExecutable<T1, (T2, T3)> fork, Func<T2, TNew> map) {
    return fork.First(Executable.Create(map));
  }

  [Pure]
  public static IExecutable<T1, (T2, TNew)> Second<T1, T2, T3, TNew>(this IExecutable<T1, (T2, T3)> fork, IExecutable<T3, TNew> map) {
    ExceptionsHelper.ThrowIfNull(map, nameof(map));
    return fork.Then(new SecondMapExecutable<T2, T3, TNew>(map));
  }

  [Pure]
  public static IExecutable<T1, (T2, TNew)> Second<T1, T2, T3, TNew>(this IExecutable<T1, (T2, T3)> fork, Func<T3, TNew> map) {
    return fork.Second(Executable.Create(map));
  }

  [Pure]
  public static IExecutable<T1, T4> Merge<T1, T2, T3, T4>(this IExecutable<T1, (T2, T3)> fork, IExecutable<(T2, T3), T4> merge) {
    return fork.Then(merge);
  }

  [Pure]
  public static IExecutable<T1, T4> Merge<T1, T2, T3, T4>(this IExecutable<T1, (T2, T3)> fork, Func<T2, T3, T4> merge) {
    ExceptionsHelper.ThrowIfNull(merge, nameof(merge));
    return fork.Then(x => merge(x.Item1, x.Item2));
  }

  [Pure]
  public static IExecutable<T1, T4> Map<T1, T2, T3, T4>(this IExecutable<T2, T3> executable, IExecutable<T1, T2> incoming, IExecutable<T3, T4> outgoing) {
    return executable.Apply(Interactions.Map.Create(incoming, outgoing));
  }

  [Pure]
  public static IExecutable<T1, T4> Map<T1, T2, T3, T4>(this IExecutable<T2, T3> executable, Func<T1, T2> incoming, Func<T3, T4> outgoing) {
    return executable.Map(Executable.Create(incoming), Executable.Create(outgoing));
  }

  [Pure]
  public static IExecutable<T1, T3> InMap<T1, T2, T3>(this IExecutable<T2, T3> executable, IExecutable<T1, T2> incoming) {
    return executable.Map(incoming, Executable.Identity<T3>());
  }

  [Pure]
  public static IExecutable<T1, T3> OutMap<T1, T2, T3>(this IExecutable<T1, T2> executable, IExecutable<T2, T3> outgoing) {
    return executable.Map(Executable.Identity<T1>(), outgoing);
  }

  [Pure]
  public static IExecutable<T1, T3> InMap<T1, T2, T3>(this IExecutable<T2, T3> executable, Func<T1, T2> incoming) {
    return executable.InMap(Executable.Create(incoming));
  }

  [Pure]
  public static IExecutable<T1, T3> OutMap<T1, T2, T3>(this IExecutable<T1, T2> executable, Func<T2, T3> outgoing) {
    return executable.OutMap(Executable.Create(outgoing));
  }

  [Pure]
  public static IExecutable<T1, T2> Tap<T1, T2>(this IExecutable<T1, T2> executable, Action<T2> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return executable.Then(new TransitiveExecutable<T2>(action));
  }

  [Pure]
  public static IAsyncExecutable<T1, T2> Tap<T1, T2>(this IExecutable<T1, T2> executable, AsyncAction<T2> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return executable.Then(new AsyncTransitiveExecutable<T2>(action));
  }

}