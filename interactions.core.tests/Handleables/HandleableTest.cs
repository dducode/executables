using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handleables;

[TestSubject(typeof(Handleable))]
public class HandleableTest {

  [Fact]
  public void PassNullArguments() {
    Assert.Throws<ArgumentNullException>(() => Handleable.Create((Func<Handler<Unit, Unit>, IDisposable>)null));
    Assert.Throws<ArgumentNullException>(() => Handleable.Create((Func<AsyncHandler<Unit, Unit>, IDisposable>)null));

    Assert.Throws<ArgumentNullException>(() => Handleable.FromEvent<Unit, Unit>(null, null));
    Assert.Throws<ArgumentNullException>(() => Handleable.FromEvent<Unit>(null, null));
    Assert.Throws<ArgumentNullException>(() => Handleable.FromEvent(null, null));
  }

  [Fact]
  public void PassNullHandler() {
    Handleable<Unit, Unit> handleable = Handleable.Create((Handler<Unit, Unit> handler) => handler);
    Assert.Throws<ArgumentNullException>(() => handleable.Handle(null));

    AsyncHandleable<Unit, Unit> asyncHandleable = Handleable.Create((AsyncHandler<Unit, Unit> handler) => handler);
    Assert.Throws<ArgumentNullException>(() => asyncHandleable.Handle(null));
  }

}