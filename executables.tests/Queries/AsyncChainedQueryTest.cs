using Executables.Core.Queries;
using Executables.Queries;
using JetBrains.Annotations;

namespace Executables.Tests.Queries;

[TestSubject(typeof(AsyncChainedQuery<,,>))]
public class AsyncChainedQueryTest {

  [Theory]
  [InlineData("00:00:10", 10)]
  [InlineData("00:01:00", 60)]
  public async Task ConnectSecondToFirst(string expected, int value) {
    IAsyncQuery<int, TimeSpan> first = AsyncExecutable
      .Create(async (int seconds, CancellationToken token) => {
        await Task.Delay(10, token);
        return TimeSpan.FromSeconds(seconds);
      })
      .AsQuery();

    IAsyncQuery<TimeSpan, string> second = AsyncExecutable
      .Create(async (TimeSpan time, CancellationToken token) => {
        await Task.Delay(10, token);
        return time.ToString();
      })
      .AsQuery();

    IAsyncQuery<int, string> chained = first.Connect(second);
    Assert.Equal(expected, await chained.Send(value));
  }

  [Theory]
  [InlineData("00:00:10", 10)]
  [InlineData("00:01:00", 60)]
  public async Task ConnectSyncToAsync(string expected, int value) {
    IQuery<int, TimeSpan> first = Executable
      .Create((int seconds) => TimeSpan.FromSeconds(seconds))
      .AsQuery();

    IAsyncQuery<TimeSpan, string> second = AsyncExecutable
      .Create(async (TimeSpan time, CancellationToken token) => {
        await Task.Delay(10, token);
        return time.ToString();
      })
      .AsQuery();

    IAsyncQuery<int, string> chained = first.Connect(second);
    Assert.Equal(expected, await chained.Send(value));
  }

  [Theory]
  [InlineData("00:00:10", 10)]
  [InlineData("00:01:00", 60)]
  public async Task ConnectAsyncToSync(string expected, int value) {
    IAsyncQuery<int, TimeSpan> first = AsyncExecutable
      .Create(async (int seconds, CancellationToken token) => {
        await Task.Delay(10, token);
        return TimeSpan.FromSeconds(seconds);
      })
      .AsQuery();

    IQuery<TimeSpan, string> second = Executable
      .Create((TimeSpan time) => time.ToString())
      .AsQuery();

    IAsyncQuery<int, string> chained = first.Connect(second);
    Assert.Equal(expected, await chained.Send(value));
  }

}