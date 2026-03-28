using Interactions.Core.Handlers;
using Interactions.Handling;
using Interactions.Tests.Utils;
using JetBrains.Annotations;

namespace Interactions.Tests.Handlers;

[TestSubject(typeof(AsyncAutoDetachableHandler<,,>))]
public class AsyncAutoDetachableHandlerTest {

  [Fact]
  public async Task DisposeOnException() {
    var query = new AsyncQuery<string, int>();

    AsyncHandler<string, int> inner = TestHandler.IntParseHandler().ToAsyncHandler();
    query.Handle(inner.DisposeExternalHandle().OnException<ArgumentNullException>());

    await Assert.ThrowsAsync<ArgumentNullException>(async () => await query.Send(null));
    await Assert.ThrowsAsync<MissingHandlerException>(async () => await query.Send(null));
  }

}