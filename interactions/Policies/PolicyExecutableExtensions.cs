using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.Policies;

public static class PolicyExecutableExtensions {

  [Pure]
  public static IExecutable<T1, T2> Apply<T1, T2>(this Policy<T1, T2> policy, IExecutable<T1, T2> executable) {
    policy.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return new ExecutablePolicy<T1, T2>(executable, policy);
  }

  [Pure]
  public static IAsyncExecutable<T1, T2> Apply<T1, T2>(this AsyncPolicy<T1, T2> policy, IAsyncExecutable<T1, T2> executable) {
    policy.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return new AsyncExecutablePolicy<T1, T2>(executable, policy);
  }

}