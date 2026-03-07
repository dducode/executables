using System.Diagnostics.Contracts;
using Interactions.Core;

namespace Interactions.Guards;

public abstract partial class Guard {

  /// <summary>
  /// Creates a no-op guard that always grants access.
  /// </summary>
  /// <returns>Guard instance that never blocks invocation.</returns>
  [Pure]
  public static Guard Identity() {
    return IdentityGuard.Instance;
  }

  [Pure]
  public static Guard Dynamic(IProvider<Guard> provider) {
    ExceptionsHelper.ThrowIfNull(provider, nameof(provider));
    return new DynamicGuard(provider);
  }

  [Pure]
  public static Guard Dynamic(Func<Guard> provider) {
    return Dynamic(Provider.Create(provider));
  }

  [Pure]
  public static Guard Lazy(IResolver<Guard> resolver) {
    ExceptionsHelper.ThrowIfNull(resolver, nameof(resolver));
    return new LazyGuard(resolver);
  }

  [Pure]
  public static Guard Lazy(Func<Guard> resolver) {
    return Lazy(Resolver.Create(resolver));
  }

  /// <summary>
  /// Creates a manually controlled guard.
  /// </summary>
  /// <param name="errorMessage">Message returned when guard denies access.</param>
  /// <returns>Guard that can be opened and closed programmatically.</returns>
  [Pure]
  public static ManualGuard Manual(string errorMessage) {
    ExceptionsHelper.ThrowIfNullOrEmpty(errorMessage, nameof(errorMessage));
    return new ManualGuard(errorMessage);
  }

  /// <summary>
  /// Creates a guard from a predicate and error message.
  /// </summary>
  /// <param name="condition">Predicate that returns true to allow access.</param>
  /// <param name="message">Error message used when access is denied.</param>
  [Pure]
  public static Guard Create(Func<bool> condition, string message) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNullOrEmpty(message, nameof(message));
    return new AnonymousGuard(condition, message);
  }

}