using Executables.Core.Handleables;
using Executables.Handling;
using Executables.Lifecycle;
using JetBrains.Annotations;

namespace Executables.Tests.Handleables;

[TestSubject(typeof(CompositeHandleable<,,>))]
public class CompositeHandleableTest {

  [Fact]
  public void PassNullHandler() {
    IHandleable<Unit, Unit> first = Handleable.Create((Handler<Unit, Unit> handler) => handler);
    IHandleable<Unit, Unit> second = Handleable.Create((Handler<Unit, Unit> handler) => handler);
    Assert.Throws<ArgumentNullException>(() => first.Merge(second).Handle(null));
  }

  [Fact]
  public void FirstHandleableThrowsException() {
    var secondCalled = false;
    IHandleable<Unit, Unit> first = Handleable.Create((Handler<Unit, Unit> _) => throw new InvalidOperationException());
    IHandleable<Unit, Unit> second = Handleable.Create((Handler<Unit, Unit> handler) => {
      secondCalled = true;
      return handler;
    });
    Assert.Throws<InvalidOperationException>(() => first.Merge(second).Handle(Executable.Identity().AsHandler()));
    Assert.False(secondCalled);
  }

  [Fact]
  public void SecondHandleableThrowsException() {
    var firstDisposed = false;
    IHandleable<Unit, Unit> first = Handleable.Create((Handler<Unit, Unit> _) => Disposable.Create(() => firstDisposed = true));
    IHandleable<Unit, Unit> second = Handleable.Create((Handler<Unit, Unit> _) => throw new InvalidOperationException());
    Assert.Throws<InvalidOperationException>(() => first.Merge(second).Handle(Executable.Identity().AsHandler()));
    Assert.True(firstDisposed);
  }

}