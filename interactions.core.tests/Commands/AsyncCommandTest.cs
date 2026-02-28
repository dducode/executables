using Interactions.Core.Commands;
using Interactions.Core.Extensions;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Commands;

[TestSubject(typeof(AsyncCommand<>))]
public class AsyncCommandTest {

  [Fact]
  public async Task ExecutionCancelTest() {
    var command = new AsyncCommand<Unit>();
    command.Handle(Handler.Identity().ToAsyncHandler());
    var cts = new CancellationTokenSource();

    Assert.True(await command.Execute(cts.Token));
    await cts.CancelAsync();
    Assert.False(await command.Execute(cts.Token));
  }

}