using Interactions.Core.Handlers;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handlers;

[TestSubject(typeof(ConditionalHandler<,>))]
public class ConditionalHandlerTest {

  [Fact]
  public void DisposeInnerHandlers() {
    Handler<Unit, Unit> mainHandler = Handler.Identity();
    Handler<Unit, Unit> otherHandler = Handler.Identity();
    Handler<Unit, Unit> handler = Branch
      .If(() => true, mainHandler)
      .Else(otherHandler);

    handler.Dispose();
    Assert.True(mainHandler.Disposed);
    Assert.True(otherHandler.Disposed);
  }

}