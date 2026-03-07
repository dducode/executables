using System.Diagnostics.Contracts;

namespace Interactions.Core;

/// <summary>
/// Resolves a single instance, typically once per lazy handler.
/// </summary>
/// <typeparam name="T">Resolved instance type.</typeparam>
public interface IResolver<out T> {

  /// <summary>
  /// Resolves an instance.
  /// </summary>
  T Resolve();

}

/// <summary>
/// Factory methods for <see cref="IResolver{T}"/>.
/// </summary>
public static class Resolver {

  /// <summary>
  /// Wraps a delegate into an <see cref="IResolver{T}"/>.
  /// </summary>
  /// <typeparam name="T">Resolved instance type.</typeparam>
  /// <param name="resolver">Delegate that resolves an instance.</param>
  [Pure]
  public static IResolver<T> Create<T>(Func<T> resolver) {
    ExceptionsHelper.ThrowIfNull(resolver, nameof(resolver));
    return new AnonymousResolver<T>(resolver);
  }

}
