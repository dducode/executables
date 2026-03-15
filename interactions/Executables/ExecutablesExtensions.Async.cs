using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Core.Internal;
using Interactions.Operations;

namespace Interactions.Executables;

public static partial class ExecutableExtensions {

  [Pure]
  public static IAsyncExecutable<T1, T3> Then<T1, T2, T3>(this IAsyncExecutable<T1, T2> first, IAsyncExecutable<T2, T3> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new AsyncCompositeExecutable<T1, T2, T3>(first, second);
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> Then<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, Func<T2, T3> next) {
    return executable.Then(Executable.Create(next));
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> Then<T1, T2, T3>(this IAsyncExecutable<T1, T2> first, IExecutable<T2, T3> second) {
    return first.Then(second.ToAsyncExecutable());
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> Then<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, AsyncFunc<T2, T3> next) {
    return executable.Then(AsyncExecutable.Create(next));
  }

  [Pure]
  public static IAsyncExecutable<T1, (T4, T5)> Then<T1, T2, T3, T4, T5>(
    this IAsyncExecutable<T1, (T2, T3)> fork,
    IAsyncExecutable<T2, T4> first,
    IAsyncExecutable<T3, T5> second) {
    ExceptionsHelper.ThrowIfNull(first, nameof(first));
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return fork.Then(new ParallelExecutable<T2, T3, T4, T5>(first, second));
  }

  [Pure]
  public static IAsyncExecutable<T1, (T2, T3)> Fork<T1, T2, T3>(this IAsyncExecutable<T1, T2> first, IAsyncExecutable<T1, T3> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new AsyncForkExecutable<T1, T2, T3>(first, second);
  }

  [Pure]
  public static IAsyncExecutable<T1, (T2, T3)> Fork<T1, T2, T3>(this IAsyncExecutable<T1, T2> first, AsyncFunc<T1, T3> second) {
    return first.Fork(AsyncExecutable.Create(second));
  }

  [Pure]
  public static IAsyncExecutable<T1, (T3, T2)> Swap<T1, T2, T3>(this IAsyncExecutable<T1, (T2, T3)> fork) {
    return fork.Then(x => (x.Item2, x.Item1));
  }

  [Pure]
  public static IAsyncExecutable<T1, (TNew, T3)> First<T1, T2, T3, TNew>(this IAsyncExecutable<T1, (T2, T3)> fork, IAsyncExecutable<T2, TNew> map) {
    ExceptionsHelper.ThrowIfNull(map, nameof(map));
    return fork.Then(new AsyncFirstMapExecutable<T2, T3, TNew>(map));
  }

  [Pure]
  public static IAsyncExecutable<T1, (TNew, T3)> First<T1, T2, T3, TNew>(this IAsyncExecutable<T1, (T2, T3)> fork, AsyncFunc<T2, TNew> map) {
    return fork.First(AsyncExecutable.Create(map));
  }

  [Pure]
  public static IAsyncExecutable<T1, (T2, TNew)> Second<T1, T2, T3, TNew>(this IAsyncExecutable<T1, (T2, T3)> fork, IAsyncExecutable<T3, TNew> map) {
    ExceptionsHelper.ThrowIfNull(map, nameof(map));
    return fork.Then(new AsyncSecondMapExecutable<T2, T3, TNew>(map));
  }

  [Pure]
  public static IAsyncExecutable<T1, (T2, TNew)> Second<T1, T2, T3, TNew>(this IAsyncExecutable<T1, (T2, T3)> fork, AsyncFunc<T3, TNew> map) {
    return fork.Second(AsyncExecutable.Create(map));
  }

  [Pure]
  public static IAsyncExecutable<T1, T4> Merge<T1, T2, T3, T4>(this IAsyncExecutable<T1, (T2, T3)> fork, IAsyncExecutable<(T2, T3), T4> merge) {
    return fork.Then(merge);
  }

  [Pure]
  public static IAsyncExecutable<T1, T4> Merge<T1, T2, T3, T4>(this IAsyncExecutable<T1, (T2, T3)> fork, AsyncFunc<T2, T3, T4> merge) {
    ExceptionsHelper.ThrowIfNull(merge, nameof(merge));
    return fork.Then(async (x, t) => await merge(x.Item1, x.Item2, t));
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> Race<T1, T2, T3>(
    this IAsyncExecutable<T1, T2> executable,
    IAsyncExecutable<T2, T3> first,
    IAsyncExecutable<T2, T3> second) {
    ExceptionsHelper.ThrowIfNull(first, nameof(first));
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return executable.Then(new RaceExecutable<T2, T3>(first, second));
  }

  [Pure]
  public static IAsyncExecutable<T1, T4> Map<T1, T2, T3, T4>(
    this IAsyncExecutable<T2, T3> executable,
    IExecutable<T1, T2> incoming,
    IExecutable<T3, T4> outgoing) {
    return executable.Apply(AsyncMap.Create(incoming, outgoing));
  }

  [Pure]
  public static IAsyncExecutable<T1, T4> Map<T1, T2, T3, T4>(this IAsyncExecutable<T2, T3> executable, Func<T1, T2> incoming, Func<T3, T4> outgoing) {
    return executable.Map(Executable.Create(incoming), Executable.Create(outgoing));
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> InMap<T1, T2, T3>(this IAsyncExecutable<T2, T3> executable, IExecutable<T1, T2> incoming) {
    return executable.Map(incoming, Executable.Identity<T3>());
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> OutMap<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, IExecutable<T2, T3> outgoing) {
    return executable.Map(Executable.Identity<T1>(), outgoing);
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> InMap<T1, T2, T3>(this IAsyncExecutable<T2, T3> executable, Func<T1, T2> incoming) {
    return executable.InMap(Executable.Create(incoming));
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> OutMap<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, Func<T2, T3> outgoing) {
    return executable.OutMap(Executable.Create(outgoing));
  }

  [Pure]
  public static IAsyncExecutable<T1, T2> Tap<T1, T2>(this IAsyncExecutable<T1, T2> executable, Action<T2> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return executable.Then(new TransitiveExecutable<T2>(action));
  }

  [Pure]
  public static IAsyncExecutable<T1, T2> Tap<T1, T2>(this IAsyncExecutable<T1, T2> executable, AsyncAction<T2> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return executable.Then(new AsyncTransitiveExecutable<T2>(action));
  }

}