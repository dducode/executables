using Interactions.Core.Handleables;
using Interactions.Handling;
using Interactions.Lifecycle;
using JetBrains.Annotations;

namespace Interactions.Tests.Core.Handleables;

[TestSubject(typeof(CompositeHandleable<,,>))]
public class CompositeHandleableTest {

  [Fact]
  public void PassNullHandler() {
    IHandleable<Unit, Unit> first = Handleable.Create((Handler<Unit, Unit> handler) => handler);
    IHandleable<Unit, Unit> second = Handleable.Create((Handler<Unit, Unit> handler) => handler);
    Assert.Throws<ArgumentNullException>(() => first.Compose(second).Handle(null));
  }

  [Fact]
  public void FirstHandleableThrowsException() {
    var secondCalled = false;
    IHandleable<Unit, Unit> first = Handleable.Create((Handler<Unit, Unit> _) => throw new InvalidOperationException());
    IHandleable<Unit, Unit> second = Handleable.Create((Handler<Unit, Unit> handler) => {
      secondCalled = true;
      return handler;
    });
    Assert.Throws<InvalidOperationException>(() => first.Compose(second).Handle(Executable.Identity().AsHandler()));
    Assert.False(secondCalled);
  }

  [Fact]
  public void SecondHandleableThrowsException() {
    var firstDisposed = false;
    IHandleable<Unit, Unit> first = Handleable.Create((Handler<Unit, Unit> _) => Disposable.Create(() => firstDisposed = true));
    IHandleable<Unit, Unit> second = Handleable.Create((Handler<Unit, Unit> _) => throw new InvalidOperationException());
    Assert.Throws<InvalidOperationException>(() => first.Compose(second).Handle(Executable.Identity().AsHandler()));
    Assert.True(firstDisposed);
  }

}