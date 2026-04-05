using System.Diagnostics.Contracts;
using Executables.Core.Policies;
using Executables.Fallbacks;
using Executables.Guards;
using Executables.Internal;
using Executables.Operations;
using Executables.Validation;

namespace Executables.Policies;

/// <summary>
/// Builds a composed policy pipeline.
/// </summary>
/// <typeparam name="T1">Input type of the target executable.</typeparam>
/// <typeparam name="T2">Output type of the target executable.</typeparam>
/// <remarks>
/// Policies are invoked in reverse order of addition: the last added policy executes first.
/// </remarks>
public readonly struct PolicyBuilder<T1, T2>() {

  private readonly List<Policy<T1, T2>> _policies = [];

  /// <summary>
  /// Adds a policy that validates input before invocation and output after invocation.
  /// </summary>
  /// <param name="inputValidator">Validator for invocation input.</param>
  /// <param name="outputValidator">Validator for invocation result.</param>
  /// <returns>Current builder instance.</returns>
  public PolicyBuilder<T1, T2> Validate(Validator<T1> inputValidator, Validator<T2> outputValidator) {
    ExceptionsHelper.ThrowIfNull(inputValidator, nameof(inputValidator));
    ExceptionsHelper.ThrowIfNull(outputValidator, nameof(outputValidator));
    return Add(new ValidationPolicy<T1, T2>(inputValidator, outputValidator));
  }

  /// <summary>
  /// Adds a policy that blocks invocation when the guard denies access.
  /// </summary>
  /// <param name="guard">Guard that controls whether invocation is allowed.</param>
  /// <returns>Current builder instance.</returns>
  public PolicyBuilder<T1, T2> Guard(Guard guard) {
    ExceptionsHelper.ThrowIfNull(guard, nameof(guard));
    return Add(new GuardPolicy<T1, T2>(guard));
  }

  /// <summary>
  /// Adds a policy that rejects reentrant execution.
  /// </summary>
  /// <returns>Current builder instance.</returns>
  public PolicyBuilder<T1, T2> PreventReentrance() {
    return Add(new PreventReentrancePolicy<T1, T2>());
  }

  /// <summary>
  /// Adds a fallback policy that handles exceptions of type <typeparamref name="TEx"/>.
  /// </summary>
  /// <typeparam name="TEx">Handled exception type.</typeparam>
  /// <param name="fallback">Fallback handler invoked after a handled exception.</param>
  /// <returns>Current builder instance.</returns>
  public PolicyBuilder<T1, T2> Fallback<TEx>(IFallbackHandler<T1, TEx, T2> fallback) where TEx : Exception {
    ExceptionsHelper.ThrowIfNull(fallback, nameof(fallback));
    return Add(new FallbackPolicy<T1, T2, TEx>(fallback));
  }

  /// <summary>
  /// Creates a fallback policy from a delegate.
  /// </summary>
  /// <typeparam name="TEx">Handled exception type.</typeparam>
  /// <param name="fallback">Delegate that converts input and exception into a fallback result.</param>
  /// <returns>Current builder instance.</returns>
  public PolicyBuilder<T1, T2> Fallback<TEx>(Func<T1, TEx, T2> fallback) where TEx : Exception {
    return Fallback(FallbackHandler.Create(fallback));
  }

  /// <summary>
  /// Adds a policy created from a delegate.
  /// </summary>
  /// <param name="policy">Delegate implementing policy behavior.</param>
  /// <returns>Current builder instance.</returns>
  public PolicyBuilder<T1, T2> Create(Func<T1, IExecutor<T1, T2>, T2> policy) {
    ExceptionsHelper.ThrowIfNull(policy, nameof(policy));
    return Add(new AnonymousPolicy<T1, T2>(policy));
  }

  /// <summary>
  /// Adds a policy to the builder pipeline.
  /// </summary>
  /// <param name="policy">Policy to add.</param>
  /// <returns>Current builder instance.</returns>
  public PolicyBuilder<T1, T2> Add(Policy<T1, T2> policy) {
    _policies.Add(policy);
    return this;
  }

  [Pure]
  internal IExecutor<T1, T2> Apply(IExecutor<T1, T2> executor) {
    return _policies.Count == 0
      ? executor
      : _policies.Aggregate(executor, (current, policy) => current.Apply(policy));
  }

}