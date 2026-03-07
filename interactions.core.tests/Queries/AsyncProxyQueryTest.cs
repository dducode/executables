using Interactions.Core.Executables;
using Interactions.Core.Handlers;
using Interactions.Core.Internal;
using Interactions.Core.Queries;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Queries;

[TestSubject(typeof(AsyncProxyExecutable<,>))]
public class AsyncProxyExecutableTest {

  [Fact]
  public async Task Cancel() {
    var cts = new CancellationTokenSource();
    var inner = new Query<Unit, Unit>();
    IAsyncExecutable<Unit, Unit> query = inner.ToAsyncExecutable();

    inner.Handle(Handler.Identity());
    await cts.CancelAsync();
    await Assert.ThrowsAsync<OperationCanceledException>(async () => await query.Execute(default, cts.Token));
  }

}