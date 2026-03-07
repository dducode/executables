using Interactions.Core.Events;
using Interactions.Core.Subscribers;

namespace Interactions.Core.Tests.Utils;

internal sealed class EventSubscriber(Action action = null) : ISubscriber<Unit> {

  internal bool Received { get; private set; }

  public Unit Execute(Unit arg) {
    Received = true;
    action?.Invoke();
    return default;
  }

}