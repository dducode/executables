using Interactions.Core;
using Interactions.Extensions;
using Interactions.Handlers;
using JetBrains.Annotations;

namespace Interactions.Tests.Handlers;

[TestSubject(typeof(AnonymousDisposeHandler<,>))]
public class AnonymousDisposeHandlerTest {

  [Fact]
  public void InvokeInnerHandler() {
    Handler<Unit, bool> handler = Handler.FromMethod(() => true).OnDispose(() => { });
    Assert.True(handler.Handle(default));
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