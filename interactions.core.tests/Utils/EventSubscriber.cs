using Interactions.Core.Events;

namespace Interactions.Core.Tests.Utils;

internal sealed class EventSubscriber(Action action = null) : ISubscriber<Unit> {

  internal bool Received { get; private set; }

  public void Receive(Unit arg) {
    Received = true;
    action?.Invoke();
  }

}