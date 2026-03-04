using Interactions.Core.Extensions;
using Interactions.Core.Handlers;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handlers;

[TestSubject(typeof(AsyncConditionalHandler<,>))]
public class AsyncConditionalHandlerTest {

  [Fact]
  public void DisposeInnerHandlers() {
    AsyncHandler<Unit, Unit> mainHandler = Handler.Identity().ToAsyncHandler();
    AsyncHandler<Unit, Unit> otherHandler = Handler.Identity().ToAsyncHandler();
    AsyncHandler<Unit, Unit> handler = Branch
      .If(() => true, mainHandler)
      .Else(otherHandler);

    handler.Dispose();
    Assert.True(mainHandler.Disposed);
    Assert.True(otherHandler.Disposed);
  }

}