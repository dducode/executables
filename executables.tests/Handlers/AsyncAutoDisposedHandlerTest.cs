using Executables.Core.Handlers;
using Executables.Handling;
using Executables.Tests.Utils;
using JetBrains.Annotations;

namespace Executables.Tests.Handlers;

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