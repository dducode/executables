using Interactions.Core.Handlers;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handleables;

[TestSubject(typeof(AsyncMergedHandleable<,>))]
public class AsyncMergedHandleableTest {

  [Fact]
  public void PassNullHandler() {
    AsyncHandleable<Unit, Unit> first = Handleable.Create((AsyncHandler<Unit, Unit> handler) => handler);
    AsyncHandleable<Unit, Unit> second = Handleable.Create((AsyncHandler<Unit, Unit> handler) => handler);
    Assert.Throws<ArgumentNullException>(() => first.Merge(second).Handle(null));
  }

  [Fact]
  public void FirstHandleableThrowsException() {
    var secondCalled = false;
    AsyncHandleable<Unit, Unit> first = Handleable.Create((AsyncHandler<Unit, Unit> _) => throw new InvalidOperationException());
    AsyncHandleable<Unit, Unit> second = Handleable.Create((AsyncHandler<Unit, Unit> handler) => {
      secondCalled = true;
      return handler;
    });
    Assert.Throws<InvalidOperationException>(() => first.Merge(second).Handle(Handler.Identity().ToAsyncHandler()));
    Assert.False(secondCalled);
  }

  [Fact]
  public void SecondHandleableThrowsException() {
    var firstDisposed = false;
    AsyncHandleable<Unit, Unit> first = Handleable.Create((AsyncHandler<Unit, Unit> _) => Disposable.Create(() => firstDisposed = true));
    AsyncHandleable<Unit, Unit> second = Handleable.Create((AsyncHandler<Unit, Unit> _) => throw new InvalidOperationException());
    Assert.Throws<InvalidOperationException>(() => first.Merge(second).Handle(Handler.Identity().ToAsyncHandler()));
    Assert.True(firstDisposed);
  }

}