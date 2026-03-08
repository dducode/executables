using Interactions.Core.Handleables;
using Interactions.Core.Handlers;
using Interactions.Core.Lifecycle;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handleables;

[TestSubject(typeof(AsyncCompositeHandleable<,,>))]
public class AsyncCompositeHandleableTest {

  [Fact]
  public void PassNullHandler() {
    IAsyncHandleable<Unit, Unit> first = AsyncHandleable.Create((AsyncHandler<Unit, Unit> handler) => handler);
    IAsyncHandleable<Unit, Unit> second = AsyncHandleable.Create((AsyncHandler<Unit, Unit> handler) => handler);
    Assert.Throws<ArgumentNullException>(() => first.Compose(second).Handle(null));
  }

  [Fact]
  public void FirstHandleableThrowsException() {
    var secondCalled = false;
    IAsyncHandleable<Unit, Unit> first = AsyncHandleable.Create((AsyncHandler<Unit, Unit> _) => throw new InvalidOperationException());
    IAsyncHandleable<Unit, Unit> second = AsyncHandleable.Create((AsyncHandler<Unit, Unit> handler) => {
      secondCalled = true;
      return handler;
    });
    Assert.Throws<InvalidOperationException>(() => first.Compose(second).Handle(Handler.Identity().ToAsyncHandler()));
    Assert.False(secondCalled);
  }

  [Fact]
  public void SecondHandleableThrowsException() {
    var firstDisposed = false;
    IAsyncHandleable<Unit, Unit> first = AsyncHandleable.Create((AsyncHandler<Unit, Unit> _) => Disposable.Create(() => firstDisposed = true));
    IAsyncHandleable<Unit, Unit> second = AsyncHandleable.Create((AsyncHandler<Unit, Unit> _) => throw new InvalidOperationException());
    Assert.Throws<InvalidOperationException>(() => first.Compose(second).Handle(Handler.Identity().ToAsyncHandler()));
    Assert.True(firstDisposed);
  }

}