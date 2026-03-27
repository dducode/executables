using Interactions.Subscribers;

namespace Interactions.Tests.Utils;

internal sealed class EventSubscriber(Action action = null) : ISubscriber<Unit>, IExecutor<Unit, Unit> {

  internal bool Received { get; private set; }

  public void Receive(Unit input) {
    Received = true;
    action?.Invoke();
  }

  public IExecutor<Unit, Unit> GetExecutor() {
    return this;
  }

  Unit IExecutor<Unit, Unit>.Execute(Unit arg) {
    Receive(arg);
    return default;
  }

}