using Interactions.Core.Handlers;
using Interactions.Handling;
using Interactions.Tests.Utils;
using JetBrains.Annotations;

namespace Interactions.Tests.Handlers;

[TestSubject(typeof(AsyncAutoDisposedHandler<,>))]
public class AsyncAutoDisposedHandlerTest {

  [Fact]
  public async Task DisposeOnUnhandledException() {
    var query = new AsyncQuery<string, int>();

    AsyncHandler<string, int> inner = TestHandler.IntParseHandler().ToAsyncHandler();
    query.Handle(inner.DisposeOnUnhandledException());

    await Assert.ThrowsAsync<ArgumentNullException>(async () => await query.Send(null));
    await Assert.ThrowsAsync<MissingHandlerException>(async () => await query.Send(null));
  }

}