using Interactions.Commands;
using Interactions.Core.Commands;
using JetBrains.Annotations;

namespace Interactions.Tests.Commands;

[TestSubject(typeof(AsyncCompositeCommand<>))]
public class AsyncCompositeCommandTest {

  [Fact]
  public async Task ComposeTwoCommands() {
    var firstExecuted = false;
    var secondExecuted = false;

    IAsyncCommand<Unit> first = AsyncExecutable
      .Create(async token => {
        await Task.Delay(10, token);
        return firstExecuted = true;
      })
      .AsCommand();

    IAsyncCommand<Unit> second = AsyncExecutable
      .Create(async token => {
        await Task.Delay(10, token);
        return secondExecuted = true;
      })
      .AsCommand();

    IAsyncCommand<Unit> composed = first.Compose(second);
    Assert.True(await composed.Execute());
    Assert.True(firstExecuted);
    Assert.True(secondExecuted);
  }

  [Fact]
  public async Task NotExecuteSecondWhenFirstIsFailed() {
    var secondExecuted = false;

    IAsyncCommand<Unit> first = AsyncExecutable
      .Create(async token => {
        await Task.Delay(10, token);
        return false;
      })
      .AsCommand();

    IAsyncCommand<Unit> second = AsyncExecutable
      .Create(_ => ValueTask.FromResult(secondExecuted = true))
      .AsCommand();

    IAsyncCommand<Unit> composed = first.Compose(second);
    Assert.False(await composed.Execute());
    Assert.False(secondExecuted);
  }

  [Fact]
  public async Task NotExecuteSecondAfterCancellation() {
    var firstExecuted = false;
    var secondExecuted = false;
    var cts = new CancellationTokenSource();

    IAsyncCommand<Unit> first = AsyncExecutable
      .Create(async token => {
        firstExecuted = true;
        await cts.CancelAsync();
        await Task.Delay(10, token);
        return true;
      })
      .AsCommand();

    IAsyncCommand<Unit> second = AsyncExecutable
      .Create(async token => {
        await Task.Delay(10, token);
        return secondExecuted = true;
      })
      .AsCommand();

    IAsyncCommand<Unit> composed = first.Compose(second);
    Assert.False(await composed.Execute(cts.Token));
    Assert.True(firstExecuted);
    Assert.False(secondExecuted);
  }

}