using System.Runtime.ExceptionServices;

namespace Interactions.Core.Events;

internal sealed class SequentialPublishHandler<T>(PublishOrder order) : Handler<Publishing<T>, Unit> {

  protected override Unit ExecuteCore(Publishing<T> publishing) {
    List<Exception> exceptions = Pool<List<Exception>>.Get();
    using var handle = new ListHandle<Exception>(exceptions);

    foreach (ISubscriber<T> subscriber in order == PublishOrder.Direct ? publishing : publishing.Reverse()) {
      try {
        subscriber.Execute(publishing.arg);
      }
      catch (Exception e) {
        exceptions.Add(e);
      }
    }

    switch (exceptions.Count) {
      case > 1:
        throw new AggregateException(exceptions);
      case 1:
        ExceptionDispatchInfo.Capture(exceptions[0]).Throw();
        break;
    }

    return default;
  }

}