using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Extensions;
using Interactions.Policies;

namespace Interactions.Extensions;

public static class ExecutableExtensions {

  [Pure]
  public static IExecutable<T1, T3> Next<T1, T2, T3>(this IExecutable<T1, T2> first, IExecutable<T2, T3> second) {
    ExceptionsHelper.ThrowIfNullReference(first);
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositeExecutable<T1, T2, T3>(first, second);
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> Next<T1, T2, T3>(this IAsyncExecutable<T1, T2> first, IAsyncExecutable<T2, T3> second) {
    ExceptionsHelper.ThrowIfNullReference(first);
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new AsyncCompositeExecutable<T1, T2, T3>(first, second);
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> Next<T1, T2, T3>(this IAsyncExecutable<T1, T2> first, IExecutable<T2, T3> second) {
    return first.Next(second.ToAsyncExecutable());
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
  public static IAsyncExecutable<T1, T3> Next<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, Func<T2, T3> next) {
    return executable.Next(Executable.Create(next));
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> Next<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, AsyncFunc<T2, T3> next) {
    return executable.Next(AsyncExecutable.Create(next));
  }

  [Pure]
  public static IExecutable<T1, T2> WithPolicy<T1, T2>(this IExecutable<T1, T2> executable, Policy<T1, T2> policy) {
    ExceptionsHelper.ThrowIfNullReference(executable);
    ExceptionsHelper.ThrowIfNull(policy, nameof(policy));
    return new PolicyExecutable<T1, T2>(executable, policy);
  }

  [Pure]
  public static IAsyncExecutable<T1, T2> WithPolicy<T1, T2>(this IAsyncExecutable<T1, T2> executable, AsyncPolicy<T1, T2> policy) {
    ExceptionsHelper.ThrowIfNullReference(executable);
    ExceptionsHelper.ThrowIfNull(policy, nameof(policy));
    return new AsyncPolicyExecutable<T1, T2>(executable, policy);
  }

}