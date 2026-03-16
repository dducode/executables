using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Executables;
using Interactions.Operations;
using JetBrains.Annotations;

namespace Interactions.Tests.Executables;

[TestSubject(typeof(RaceExecutable<,>))]
public class RaceExecutableTest {

  [Theory]
  [InlineData(10, 100, 2, 1)]
  [InlineData(100, 10, 11, 1)]
  public async Task SimpleRace(int firstDelay, int secondDelay, int expected, int value) {
    IAsyncQuery<int, int> query = AsyncExecutable
      .Identity<int>()
      .Race(
        async (x, token) => {
          await Task.Delay(firstDelay, token);
          return x * 2;
        },
        async (x, token) => {
          await Task.Delay(secondDelay, token);
          return x + 10;
        })
      .AsQuery();

    Assert.Equal(expected, await query.Send(value));
  }

  [Fact]
  public async Task RaceWithOnceSynchronouslyTask() {
    IAsyncQuery<int, int> query = AsyncExecutable
      .Identity<int>()
      .Race(async (x, token) => {
        await Task.Delay(10, token);
        return x * 2;
      }, (x, _) => new ValueTask<int>(x + 10))
      .AsQuery();

    Assert.Equal(11, await query.Send(1));
  }

  [Fact]
  public async Task RaceWithLoserCancellation() {
    var canceled = false;

    IAsyncQuery<int, int> query = AsyncExecutable
      .Identity<int>()
      .Race(
        async (x, token) => {
          try {
            await Task.Delay(100, token);
          }
          catch (OperationCanceledException) {
            canceled = true;
          }

          return x + 10;
        },
        async (x, token) => {
          await Task.Delay(10, token);
          return x * 2;
        })
      .Apply(ExecutionOperator.CancelAfterCompletion<int>())
      .AsQuery();

    Assert.Equal(2, await query.Send(1));
    Assert.True(canceled);
  }

  [Fact]
  public async Task ManyRace() {
    IAsyncQuery<int, int> query = AsyncExecutable
      .Identity<int>()
      .Race(
        async (x, token) => {
          await Task.Delay(100, token);
          return x * 2;
        },
        async (x, token) => {
          await Task.Delay(250, token);
          return x + 10;
        },
        async (x, token) => {
          await Task.Delay(10, token);
          return x * 10;
        },
        async (x, token) => {
          await Task.Delay(2500, token);
          return x + 100;
        },
        async (x, token) => {
          await Task.Delay(1000, token);
          return x * 100;
        })
      .AsQuery();

    Assert.Equal(10, await query.Send(1));
  }

}