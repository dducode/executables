using System.Diagnostics.Contracts;
using Executables.Core.Subscribers;
using Executables.Internal;

namespace Executables.Subscribers;

/// <summary>
/// Factory methods for creating subscribers.
/// </summary>
public static class Subscriber {

  /// <summary>
  /// Creates a subscriber from an action that receives published values.
  /// </summary>
  /// <typeparam name="T">Type of received event value.</typeparam>
  /// <param name="action">Action executed for each published value.</param>
  /// <returns>Subscriber that delegates handling to <paramref name="action"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static ISubscriber<T> Create<T>(Action<T> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new AnonymousSubscriber<T>(action);
  }

  /// <summary>
  /// Creates a subscriber from a parameterless action.
  /// </summary>
  /// <param name="action">Action executed for each published unit value.</param>
  /// <returns>Subscriber that delegates handling to <paramref name="action"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static ISubscriber<Unit> Create(Action action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new AnonymousSubscriber(action);
  }

}