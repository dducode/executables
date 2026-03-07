using System.Diagnostics.Contracts;
using Interactions.Core.Executables;
using Interactions.Core.Internal;

namespace Interactions.Policies;

public static class PolicyExecutableExtensions {

  [Pure]
  public static IExecutable<T1, T2> WithPolicy<T1, T2>(this IExecutable<T1, T2> executable, Policy<T1, T2> policy) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(policy, nameof(policy));
    return new PolicyExecutable<T1, T2>(executable, policy);
  }

  [Pure]
  public static IAsyncExecutable<T1, T2> WithPolicy<T1, T2>(this IAsyncExecutable<T1, T2> executable, AsyncPolicy<T1, T2> policy) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(policy, nameof(policy));
    return new AsyncPolicyExecutable<T1, T2>(executable, policy);
  }

}