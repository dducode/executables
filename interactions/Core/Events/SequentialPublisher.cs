using System.Runtime.ExceptionServices;
using Interactions.Events;
using Interactions.Handling;
using Interactions.Internal;

namespace Interactions.Core.Events;

internal abstract class SequentialPublisher<T> : Handler<Publishing<T>, Unit> {

  protected override Unit HandleCore(Publishing<T> publishing) {
    List<Exception> exceptions = Pool<List<Exception>>.Get();
    using var handle = new ListHandle<Exception>(exceptions);

    Publish(publishing, exceptions);

    switch (exceptions.Count) {
      case > 1:
        throw new AggregateException(exceptions);
      case 1:
        ExceptionDispatchInfo.Capture(exceptions[0]).Throw();
        break;
    }

    return default;
  }

  protected abstract void Publish(Publishing<T> publishing, List<Exception> exceptions);

}