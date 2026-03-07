using Interactions.Core.Handlers;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handlers;

[TestSubject(typeof(AnonymousDisposeHandler<,>))]
public class AnonymousDisposeHandlerTest {

  [Fact]
  public void InvokeInnerHandler() {
    Handler<Unit, bool> handler = Handler.FromMethod(() => true).OnDispose(() => { });
    Assert.True(handler.Execute(default));
  }

  [Fact]
  public void DisposeHandler() {
    var disposed = false;
    Handler<Unit, Unit> inner = Handler.Identity();
    Handler<Unit, Unit> handler = inner.OnDispose(() => disposed = true);

    handler.Dispose();

    Assert.True(disposed);
    Assert.True(inner.Disposed);
  }

}