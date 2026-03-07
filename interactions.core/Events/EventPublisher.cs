using System.Diagnostics.Contracts;

namespace Interactions.Core.Events;

public static class EventPublisher {

  [Pure]
  public static Handler<Publishing<T>, Unit> Sequential<T>(PublishOrder order = PublishOrder.Direct) {
    return new SequentialPublishHandler<T>(order);
  }

  [Pure]
  public static Handler<Publishing<Unit>, Unit> Sequential(PublishOrder order = PublishOrder.Direct) {
    return Sequential<Unit>(order);
  }

  [Pure]
  public static Handler<Publishing<T>, Unit> Parallel<T>(ParallelOptions options = null) {
    return new ParallelPublishHandler<T>(options ?? new ParallelOptions());
  }

  [Pure]
  public static Handler<Publishing<Unit>, Unit> Parallel(ParallelOptions options = null) {
    return Parallel<Unit>(options);
  }

}