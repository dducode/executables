namespace Interactions.Core.Tests.Utils;

internal class EventSubscriber : Handler<Unit, Unit> {

  internal bool Receive { get; private set; }

  protected override Unit HandleCore(Unit input) {
    Receive = true;
    return default;
  }

}

internal sealed class ThrowingExceptionSubscriber : EventSubscriber {

  protected override Unit HandleCore(Unit input) {
    base.HandleCore(input);
    throw new InvalidOperationException("Exception from subscriber");
  }

}