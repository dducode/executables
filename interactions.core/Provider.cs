using System.Diagnostics.Contracts;

namespace Interactions.Core;

/// <summary>
/// Produces a new instance per call and may hold disposable resources.
/// </summary>
/// <typeparam name="T">Provided instance type.</typeparam>
public interface IProvider<out T> : IDisposable {

  /// <summary>
  /// Produces a new instance.
  /// </summary>
  T Get();

}

/// <summary>
/// Factory methods for <see cref="IProvider{T}"/>.
/// </summary>
public static class Provider {

  /// <summary>
  /// Wraps a delegate into an <see cref="IProvider{T}"/>.
  /// </summary>
  /// <typeparam name="T">Provided instance type.</typeparam>
  /// <param name="provider">Delegate that creates an instance.</param>
  [Pure]
  public static IProvider<T> Create<T>(Func<T> provider) {
    ExceptionsHelper.ThrowIfNull(provider, nameof(provider));
    return new AnonymousProvider<T>(provider);
  }

}
