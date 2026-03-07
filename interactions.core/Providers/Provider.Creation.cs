using System.Diagnostics.Contracts;
using Interactions.Core.Internal;
using Interactions.Core.Providers;

namespace Interactions.Core;

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