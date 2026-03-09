using Interactions.Core.Executables;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handlers;

[TestSubject(typeof(Handler<,>))]
public class HandlerTest {

  [Fact]
  public void DisposeHandler() {
    Handler<Unit, Unit> handler = Executable.Identity().AsHandler();
    handler.Dispose();
    Assert.True(handler.Disposed);
  }

}