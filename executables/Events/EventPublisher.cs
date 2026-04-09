using System.Diagnostics.Contracts;
using Executables.Core.Events;
using Executables.Handling;

namespace Executables.Events;

/// <summary>
/// Factory methods for creating event publishing handlers.
/// </summary>
public static class EventPublisher {

  /// <summary>
  /// Creates a handler that publishes values to subscribers sequentially.
  /// </summary>
  /// <typeparam name="T">Type of published event value.</typeparam>
  /// <param name="order">Order in which subscribers are notified.</param>
  /// <returns>Handler that publishes values sequentially.</returns>
  /// <exception cref="ArgumentOutOfRangeException"><paramref name="order"/> is not a valid <see cref="PublishOrder"/> value.</exception>
  [Pure]
  public static Handler<Publishing<T>, Unit> Sequential<T>(PublishOrder order = PublishOrder.Direct) {
    return order switch {
      PublishOrder.Direct => new DirectPublisher<T>(),
      PublishOrder.Reverse => new ReversedPublisher<T>(),
      _ => throw new ArgumentOutOfRangeException(nameof(order))
    };
  }

  /// <summary>
  /// Creates a handler that publishes notifications to subscribers sequentially.
  /// </summary>
  /// <param name="order">Order in which subscribers are notified.</param>
  /// <returns>Handler that publishes notifications sequentially.</returns>
  /// <exception cref="ArgumentOutOfRangeException"><paramref name="order"/> is not a valid <see cref="PublishOrder"/> value.</exception>
  [Pure]
  public static Handler<Publishing<Unit>, Unit> Sequential(PublishOrder order = PublishOrder.Direct) {
    return Sequential<Unit>(order);
  }

  /// <summary>
  /// Creates a handler that publishes values to subscribers in parallel.
  /// </summary>
  /// <typeparam name="T">Type of published event value.</typeparam>
  /// <param name="options">Parallel execution options, or <see langword="null"/> to use default options.</param>
  /// <returns>Handler that publishes values in parallel.</returns>
  [Pure]
  public static Handler<Publishing<T>, Unit> Parallel<T>(ParallelOptions options = null) {
    return new ParallelPublisher<T>(options ?? new ParallelOptions());
  }

  /// <summary>
  /// Creates a handler that publishes notifications to subscribers in parallel.
  /// </summary>
  /// <param name="options">Parallel execution options, or <see langword="null"/> to use default options.</param>
  /// <returns>Handler that publishes notifications in parallel.</returns>
  [Pure]
  public static Handler<Publishing<Unit>, Unit> Parallel(ParallelOptions options = null) {
    return Parallel<Unit>(options);
  }

}