using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Internal;
using Interactions.Fallbacks;
using Interactions.Guards;
using Interactions.Operations;
using Interactions.Validation;

namespace Interactions.Policies;

public class PolicyBuilder<T1, T2> {

  private readonly List<Policy<T1, T2>> _policies = [];

  internal PolicyBuilder() { }

  /// <summary>
  /// Creates a no-op policy that only executes wrapped invocation.
  /// </summary>
  /// <returns>Policy instance that does not alter invocation behavior.</returns>
  public PolicyBuilder<T1, T2> Identity() {
    return Add(IdentityPolicy<T1, T2>.Instance);
  }

  /// <summary>
  /// Creates a policy that validates input before invocation and output after invocation.
  /// </summary>
  /// <param name="inputValidator">Validator for invocation input.</param>
  /// <param name="outputValidator">Validator for invocation result.</param>
  /// <returns>Validation policy for both input and output.</returns>
  public PolicyBuilder<T1, T2> Validate(Validator<T1> inputValidator, Validator<T2> outputValidator) {
    ExceptionsHelper.ThrowIfNull(inputValidator, nameof(inputValidator));
    ExceptionsHelper.ThrowIfNull(outputValidator, nameof(outputValidator));
    return Add(new ValidationPolicy<T1, T2>(inputValidator, outputValidator));
  }

  /// <summary>
  /// Creates a policy that blocks invocation when the guard denies access.
  /// </summary>
  /// <param name="guard">Guard that controls whether invocation is allowed.</param>
  /// <returns>Guard policy.</returns>
  public PolicyBuilder<T1, T2> Guard(Guard guard) {
    ExceptionsHelper.ThrowIfNull(guard, nameof(guard));
    return Add(new GuardPolicy<T1, T2>(guard));
  }

  public PolicyBuilder<T1, T2> PreventReentrance() {
    return Add(new PreventReentrancePolicy<T1, T2>());
  }

  public PolicyBuilder<T1, T2> Fallback<TEx>(IFallbackHandler<T1, TEx, T2> fallback) where TEx : Exception {
    ExceptionsHelper.ThrowIfNull(fallback, nameof(fallback));
    return Add(new FallbackPolicy<T1, T2, TEx>(fallback));
  }

  public PolicyBuilder<T1, T2> Create(Func<T1, IExecutable<T1, T2>, T2> policy) {
    ExceptionsHelper.ThrowIfNull(policy, nameof(policy));
    return Add(new AnonymousPolicy<T1, T2>(policy));
  }

  public PolicyBuilder<T1, T2> Add(Policy<T1, T2> policy) {
    _policies.Add(policy);
    return this;
  }

  [Pure]
  public IExecutable<T1, T2> Apply(IExecutable<T1, T2> executable) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return _policies
      .AsEnumerable()
      .Reverse()
      .Aggregate(executable, (current, policy) => new ExecutableOperator<T1, T1, T2, T2>(policy, current));
  }

}