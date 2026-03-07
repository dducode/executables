using Interactions.Core.Handlers;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handlers;

[TestSubject(typeof(AsyncProxyHandler<,>))]
public class AsyncProxyHandlerTest {

  [Fact]
  public void DisposeInnerHandler() {
    Handler<Unit, Unit> inner = Handler.Identity();
    AsyncHandler<Unit, Unit> handler = inner.ToAsyncHandler();
    handler.Dispose();
    Assert.True(inner.Disposed);
  }

}