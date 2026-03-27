using Interactions.Core.Handleables;
using Interactions.Handling;
using Interactions.Lifecycle;
using JetBrains.Annotations;

namespace Interactions.Tests.Handleables;

[TestSubject(typeof(AsyncCompositeHandleable<,,>))]
public class AsyncCompositeHandleableTest {

  [Fact]
  public void PassNullHandler() {
    IAsyncHandleable<Unit, Unit> first = AsyncHandleable.Create((AsyncHandler<Unit, Unit> handler) => handler);
    IAsyncHandleable<Unit, Unit> second = AsyncHandleable.Create((AsyncHandler<Unit, Unit> handler) => handler);
    Assert.Throws<ArgumentNullException>(() => first.Merge(second).Handle(null));
  }

  [Fact]
  public void FirstHandleableThrowsException() {
    var secondCalled = false;
    IAsyncHandleable<Unit, Unit> first = AsyncHandleable.Create((AsyncHandler<Unit, Unit> _) => throw new InvalidOperationException());
    IAsyncHandleable<Unit, Unit> second = AsyncHandleable.Create((AsyncHandler<Unit, Unit> handler) => {
      secondCalled = true;
      return handler;
    });
    Assert.Throws<InvalidOperationException>(() => first.Merge(second).Handle(AsyncExecutable.Identity().AsHandler()));
    Assert.False(secondCalled);
  }

  [Fact]
  public void SecondHandleableThrowsException() {
    var firstDisposed = false;
    IAsyncHandleable<Unit, Unit> first = AsyncHandleable.Create((AsyncHandler<Unit, Unit> _) => Disposable.Create(() => firstDisposed = true));
    IAsyncHandleable<Unit, Unit> second = AsyncHandleable.Create((AsyncHandler<Unit, Unit> _) => throw new InvalidOperationException());
    Assert.Throws<InvalidOperationException>(() => first.Merge(second).Handle(AsyncExecutable.Identity().AsHandler()));
    Assert.True(firstDisposed);
  }

}