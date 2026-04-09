using Executables.Subscribers;

namespace Executables.Events;

/// <summary>
/// Represents a publishing operation that carries a value and the target subscribers.
/// </summary>
/// <typeparam name="T">Type of published event value.</typeparam>
public readonly struct Publishing<T>(T arg, List<ISubscriber<T>> subscribers) {

  /// <summary>
  /// Value being published.
  /// </summary>
  public readonly T arg = arg;
  /// <summary>
  /// Subscribers that should receive the published value.
  /// </summary>
  public readonly List<ISubscriber<T>> subscribers = subscribers;

}