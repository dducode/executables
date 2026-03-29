using Executables.Handling;
using JetBrains.Annotations;

namespace Executables.Tests.Handlers;

[TestSubject(typeof(Handler<,>))]
public class HandlerTest {

  [Fact]
  public void DisposeHandler() {
    Handler<Unit, Unit> handler = Executable.Identity().AsHandler();
    handler.Dispose();
    Assert.True(handler.Disposed);
  }

}