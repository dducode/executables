using Interactions.Core.Handlers;
using Interactions.Handling;
using JetBrains.Annotations;

namespace Interactions.Tests.Handlers;

[TestSubject(typeof(AnonymousDisposeHandler<,>))]
public class AnonymousDisposeHandlerTest {

  [Fact]
  public void InvokeInnerHandler() {
    Handler<Unit, bool> handler = Executable.Create(() => true).AsHandler().OnDispose(() => { });
    Assert.True(handler.Handle(default));
  }

  [Fact]
  public void WrapperDisposesInner() {
    var disposed = false;
    Handler<Unit, Unit> inner = Executable.Identity().AsHandler();
    Handler<Unit, Unit> wrapper = inner.OnDispose(() => disposed = true);

    wrapper.Dispose();

    Assert.True(disposed);
    Assert.True(inner.Disposed);
    Assert.True(wrapper.Disposed);
  }

  [Fact]
  public void InnerDisposesWrapper() {
    var disposed = false;
    Handler<Unit, Unit> inner = Executable.Identity().AsHandler();
    Handler<Unit, Unit> wrapper = inner.OnDispose(() => disposed = true);

    inner.Dispose();

    Assert.True(disposed);
    Assert.True(wrapper.Disposed);
    Assert.True(inner.Disposed);
  }

}