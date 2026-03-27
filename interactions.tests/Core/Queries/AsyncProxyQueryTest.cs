using Interactions.Core.Queries;
using Interactions.Queries;
using JetBrains.Annotations;

namespace Interactions.Tests.Core.Queries;

[TestSubject(typeof(AsyncProxyQuery<,>))]
public class AsyncProxyQueryTest {

  [Fact]
  public async Task Cancel() {
    var cts = new CancellationTokenSource();
    var inner = new Query<Unit, Unit>();
    IAsyncQuery<Unit, Unit> query = inner.ToAsyncQuery();

    inner.Handle(Executable.Identity().AsHandler());
    await cts.CancelAsync();
    await Assert.ThrowsAsync<OperationCanceledException>(async () => await query.Send(cts.Token));
  }

}