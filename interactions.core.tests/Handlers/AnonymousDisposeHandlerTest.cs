using Interactions.Core.Executables;
using Interactions.Core.Handlers;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handlers;

[TestSubject(typeof(AnonymousDisposeHandler<,>))]
public class AnonymousDisposeHandlerTest {

  [Fact]
  public void InvokeInnerHandler() {
    Handler<Unit, bool> handler = Executable.Create(() => true).AsHandler().OnDispose(() => { });
    Assert.True(handler.Execute(default));
  }

  [Fact]
  public void DisposeHandler() {
    var disposed = false;
    Handler<Unit, Unit> inner = Executable.Identity().AsHandler();
    Handler<Unit, Unit> handler = inner.OnDispose(() => disposed = true);

    handler.Dispose();

    Assert.True(disposed);
    Assert.True(inner.Disposed);
  }

}