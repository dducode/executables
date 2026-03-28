using Interactions.Events;

namespace Interactions.Core.Events;

internal sealed class ReversedPublisher<T> : SequentialPublisher<T> {

  protected override void Publish(Publishing<T> publishing, List<Exception> exceptions) {
    for (int i = publishing.subscribers.Count - 1; i >= 0; i--) {
      try {
        publishing.subscribers[i].Receive(publishing.arg);
      }
      catch (Exception e) {
        exceptions.Add(e);
      }
    }
  }

}