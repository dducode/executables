using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Core.Internal;

namespace Interactions.Executables;

public static partial class ExecutablesExtensions {

  [Pure]
  public static IExecutable<T1, T3> Next<T1, T2, T3>(this IExecutable<T1, T2> first, IExecutable<T2, T3> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositeExecutable<T1, T2, T3>(first, second);
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> Next<T1, T2, T3>(this IExecutable<T1, T2> first, IAsyncExecutable<T2, T3> second) {
    return first.ToAsyncExecutable().Next(second);
  }

  [Pure]
  public static IExecutable<T1, T3> Next<T1, T2, T3>(this IExecutable<T1, T2> executable, Func<T2, T3> next) {
    return executable.Next(Executable.Create(next));
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> Next<T1, T2, T3>(this IExecutable<T1, T2> executable, AsyncFunc<T2, T3> next) {
    return executable.ToAsyncExecutable().Next(AsyncExecutable.Create(next));
  }

  [Pure]
  public static IExecutable<T1, T4> Zip<T1, T2, T3, T4>(this IExecutable<T1, T2> first, IExecutable<T1, T3> second, IAggregator<T2, T3, T4> aggregator) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    ExceptionsHelper.ThrowIfNull(aggregator, nameof(aggregator));
    return new ZipExecutable<T1, T2, T3, T4>(first, second, aggregator);
  }

  [Pure]
  public static IExecutable<T1, T4> Zip<T1, T2, T3, T4>(this IExecutable<T1, T2> first, IExecutable<T1, T3> second, Func<T2, T3, T4> aggregation) {
    return first.Zip(second, Aggregator.Create(aggregation));
  }

  [Pure]
  public static IExecutable<T1, (T2, T3)> Zip<T1, T2, T3>(this IExecutable<T1, T2> first, IExecutable<T1, T3> second) {
    return first.Zip(second, Aggregator.Tuple<T2, T3>());
  }

}