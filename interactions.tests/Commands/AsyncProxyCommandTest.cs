using Interactions.Commands;
using Interactions.Core.Commands;
using JetBrains.Annotations;

namespace Interactions.Tests.Commands;

[TestSubject(typeof(AsyncProxyCommand<>))]
public class AsyncProxyCommandTest {

  [Fact]
  public async Task Cancel() {
    var cts = new CancellationTokenSource();
    var inner = new Command<Unit>();
    IAsyncCommand<Unit> command = inner.ToAsyncCommand();

    inner.Handle(Executable.Identity().AsHandler());
    await cts.CancelAsync();
    Assert.False(await command.Execute(cts.Token));
  }

}