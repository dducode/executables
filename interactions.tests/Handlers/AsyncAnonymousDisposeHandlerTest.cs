using Interactions.Core;
using Interactions.Core.Extensions;
using Interactions.Extensions;
using Interactions.Handlers;
using JetBrains.Annotations;

namespace Interactions.Tests.Handlers;

[TestSubject(typeof(AsyncAnonymousDisposeHandler<,>))]
public class AsyncAnonymousDisposeHandlerTest {

  [Fact]
  public async Task InvokeInnerHandler() {
    AsyncHandler<Unit, bool> handler = Handler.FromAsyncMethod(_ => new ValueTask<bool>(true)).OnDispose(() => { });
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