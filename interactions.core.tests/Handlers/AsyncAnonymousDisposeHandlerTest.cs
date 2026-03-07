using Interactions.Core.Handlers;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handlers;

[TestSubject(typeof(AsyncAnonymousDisposeHandler<,>))]
public class AsyncAnonymousDisposeHandlerTest {

  [Fact]
  public async Task InvokeInnerHandler() {
    AsyncHandler<Unit, bool> handler = AsyncHandler.Create(_ => new ValueTask<bool>(true)).OnDispose(() => { });
    Assert.True(await handler.Execute(default));
  }

  [Fact]
  public void DisposeHandler() {
    var disposed = false;
    AsyncHandler<Unit, Unit> inner = Handler.Identity().ToAsyncHandler();
    AsyncHandler<Unit, Unit> handler = inner.OnDispose(() => disposed = true);

    handler.Dispose();

    Assert.True(disposed);
    Assert.True(inner.Disposed);
  }

}