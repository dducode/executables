using Interactions.Core.Extensions;
using Interactions.Core.Handlers;
using Interactions.Core.Queries;
using Interactions.Core.Tests.Utils;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handlers;

[TestSubject(typeof(AsyncAutoDisposeHandler<,,>))]
public class AsyncAutoDisposeHandlerTest {

  [Fact]
  public async Task DisposeOnException() {
    var handle = new DisposeHandle();
    var query = new AsyncQuery<string, int>();

    AsyncHandler<string, int> inner = TestHandler.IntParseHandler().ToAsyncHandler();
    handle.Register(query.Handle(inner.DisposeOnException<ArgumentNullException>(handle)));

    await Assert.ThrowsAsync<ArgumentNullException>(async () => await query.Execute(null));
    await Assert.ThrowsAsync<MissingHandlerException>(async () => await query.Execute(null));
  }

}