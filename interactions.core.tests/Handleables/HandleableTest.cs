using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handleables;

[TestSubject(typeof(Handleable))]
public class HandleableTest {

  [Fact]
  public void PassNullArguments() {
    Assert.Throws<ArgumentNullException>(() => Handleable.Create<Unit, Unit>(null));
    Assert.Throws<ArgumentNullException>(() => AsyncHandleable.Create<Unit, Unit>(null));

    Assert.Throws<ArgumentNullException>(() => Handleable.FromEvent<Unit, Unit>(null, null));
    Assert.Throws<ArgumentNullException>(() => Handleable.FromEvent<Unit>(null, null));
    Assert.Throws<ArgumentNullException>(() => Handleable.FromEvent(null, null));
  }

  [Fact]
  public void PassNullHandler() {
    IHandleable<Unit, Unit> handleable = Handleable.Create((Handler<Unit, Unit> handler) => handler);
    Assert.Throws<ArgumentNullException>(() => handleable.Handle(null));

    IAsyncHandleable<Unit, Unit> asyncHandleable = AsyncHandleable.Create((AsyncHandler<Unit, Unit> handler) => handler);
    Assert.Throws<ArgumentNullException>(() => asyncHandleable.Handle(null));
  }

}