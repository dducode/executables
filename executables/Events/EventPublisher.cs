using System.Diagnostics.Contracts;
using Executables.Core.Events;
using Executables.Handling;

namespace Executables.Events;

public static class EventPublisher {

  [Pure]
  public static Handler<Publishing<T>, Unit> Sequential<T>(PublishOrder order = PublishOrder.Direct) {
    return order switch {
      PublishOrder.Direct => new DirectPublisher<T>(),
      PublishOrder.Reverse => new ReversedPublisher<T>(),
      _ => throw new ArgumentOutOfRangeException(nameof(order))
    };
  }

  [Pure]
  public static Handler<Publishing<Unit>, Unit> Sequential(PublishOrder order = PublishOrder.Direct) {
    return Sequential<Unit>(order);
  }

  [Pure]
  public static Handler<Publishing<T>, Unit> Parallel<T>(ParallelOptions options = null) {
    return new ParallelPublisher<T>(options ?? new ParallelOptions());
  }

  [Pure]
  public static Handler<Publishing<Unit>, Unit> Parallel(ParallelOptions options = null) {
    return Parallel<Unit>(options);
  }

}