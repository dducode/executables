using Interactions.Core.Extensions;
using Interactions.Core.Queries;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Queries;

[TestSubject(typeof(AsyncProxyQuery<,>))]
public class AsyncProxyQueryTest {

  [Fact]
  public async Task Cancel() {
    var cts = new CancellationTokenSource();
    var inner = new Query<Unit, Unit>();
    IAsyncQuery<Unit, Unit> query = inner.ToAsyncQuery();

    inner.Handle(Handler.Identity());
    await cts.CancelAsync();
    await Assert.ThrowsAsync<OperationCanceledException>(async () => await query.Send(default, cts.Token));
  }

}