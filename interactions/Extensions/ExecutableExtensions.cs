using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Policies;

namespace Interactions.Extensions;

public static class ExecutableExtensions {

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