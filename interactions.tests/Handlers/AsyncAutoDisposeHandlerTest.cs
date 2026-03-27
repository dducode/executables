using Interactions.Core.Handlers;
using Interactions.Handling;
using Interactions.Lifecycle;
using Interactions.Tests.Utils;
using JetBrains.Annotations;

namespace Interactions.Tests.Handlers;

[TestSubject(typeof(AsyncAutoDisposeHandler<,,>))]
public class AsyncAutoDisposeHandlerTest {

  [Fact]
  public async Task DisposeOnException() {
    var handle = new DisposeHandle();
    var query = new AsyncQuery<string, int>();

    AsyncHandler<string, int> inner = TestHandler.IntParseHandler().ToAsyncHandler();
    handle.Register(query.Handle(inner.DisposeExternalHandle(handle).OnException<ArgumentNullException>()));

    await Assert.ThrowsAsync<ArgumentNullException>(async () => await query.Send(null));
    await Assert.ThrowsAsync<MissingHandlerException>(async () => await query.Send(null));
  }

}