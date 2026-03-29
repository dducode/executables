using Executables.Events;
using Executables.Subscribers;

namespace Executables.Core.Events;

internal sealed class DirectPublisher<T> : SequentialPublisher<T> {

  protected override void Publish(Publishing<T> publishing, List<Exception> exceptions) {
    foreach (ISubscriber<T> subscriber in publishing.subscribers) {
      try {
        subscriber.Receive(publishing.arg);
      }
      catch (Exception e) {
        exceptions.Add(e);
      }
    }
  }

}