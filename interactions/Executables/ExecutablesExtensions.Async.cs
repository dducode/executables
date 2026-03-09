using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Core.Internal;
using Interactions.Maps;

namespace Interactions.Executables;

public static partial class ExecutablesExtensions {

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
  public static IAsyncExecutable<T1, T4> Flow<T1, T2, T3, T4>(
    this IAsyncExecutable<T1, T2> first,
    IAsyncExecutable<T1, T3> second,
    IAggregator<T2, T3, T4> aggregator) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    ExceptionsHelper.ThrowIfNull(aggregator, nameof(aggregator));
    return new AsyncFlowExecutable<T1, T2, T3, T4>(first, second, aggregator);
  }

  [Pure]
  public static IAsyncExecutable<T1, T4> Flow<T1, T2, T3, T4>(
    this IAsyncExecutable<T1, T2> first,
    IAsyncExecutable<T1, T3> second,
    Func<T2, T3, T4> aggregation) {
    return first.Flow(second, Aggregator.Create(aggregation));
  }

  [Pure]
  public static IAsyncExecutable<T1, T4> Map<T1, T2, T3, T4>(
    this IAsyncExecutable<T2, T3> executable,
    Transformer<T1, T2> incoming,
    Transformer<T3, T4> outgoing) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(incoming, nameof(incoming));
    ExceptionsHelper.ThrowIfNull(outgoing, nameof(outgoing));
    return new AsyncExecutableMap<T1, T2, T3, T4>(AsyncMap.Create(incoming, outgoing), executable);
  }

  [Pure]
  public static IAsyncExecutable<T1, T4> Map<T1, T2, T3, T4>(this IAsyncExecutable<T2, T3> executable, Func<T1, T2> incoming, Func<T3, T4> outgoing) {
    return executable.Map(Transformer.Create(incoming), Transformer.Create(outgoing));
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> InMap<T1, T2, T3>(this IAsyncExecutable<T2, T3> executable, Transformer<T1, T2> incoming) {
    return executable.Map(incoming, Transformer.Identity<T3>());
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> OutMap<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, Transformer<T2, T3> outgoing) {
    return executable.Map(Transformer.Identity<T1>(), outgoing);
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> InMap<T1, T2, T3>(this IAsyncExecutable<T2, T3> executable, Func<T1, T2> incoming) {
    return executable.InMap(Transformer.Create(incoming));
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> OutMap<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, Func<T2, T3> outgoing) {
    return executable.OutMap(Transformer.Create(outgoing));
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