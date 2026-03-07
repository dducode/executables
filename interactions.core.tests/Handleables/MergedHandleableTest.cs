using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handleables;

[TestSubject(typeof(MergedHandleable<,>))]
public class MergedHandleableTest {

  [Fact]
  public void PassNullHandler() {
    Handleable<Unit, Unit> first = Handleable.Create((Handler<Unit, Unit> handler) => handler);
    Handleable<Unit, Unit> second = Handleable.Create((Handler<Unit, Unit> handler) => handler);
    Assert.Throws<ArgumentNullException>(() => first.Merge(second).Handle(null));
  }

  [Fact]
  public void FirstHandleableThrowsException() {
    var secondCalled = false;
    Handleable<Unit, Unit> first = Handleable.Create((Handler<Unit, Unit> _) => throw new InvalidOperationException());
    Handleable<Unit, Unit> second = Handleable.Create((Handler<Unit, Unit> handler) => {
      secondCalled = true;
      return handler;
    });
    Assert.Throws<InvalidOperationException>(() => first.Merge(second).Handle(Handler.Identity()));
    Assert.False(secondCalled);
  }

  [Fact]
  public void SecondHandleableThrowsException() {
    var firstDisposed = false;
    Handleable<Unit, Unit> first = Handleable.Create((Handler<Unit, Unit> _) => Disposable.Create(() => firstDisposed = true));
    Handleable<Unit, Unit> second = Handleable.Create((Handler<Unit, Unit> _) => throw new InvalidOperationException());
    Assert.Throws<InvalidOperationException>(() => first.Merge(second).Handle(Handler.Identity()));
    Assert.True(firstDisposed);
  }

}