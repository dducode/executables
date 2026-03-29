using Executables.Core.Handlers;
using Executables.Handling;
using JetBrains.Annotations;

namespace Executables.Tests.Handlers;

[TestSubject(typeof(AsyncProxyHandler<,>))]
public class AsyncProxyHandlerTest {

  [Fact]
  public void DisposeInnerHandler() {
    Handler<Unit, Unit> inner = Executable.Identity().AsHandler();
    AsyncHandler<Unit, Unit> handler = inner.ToAsyncHandler();
    handler.Dispose();
    Assert.True(inner.Disposed);
  }

}