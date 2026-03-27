using Interactions.Core.Policies;
using Interactions.Policies;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(TimeoutPolicy<,>))]
public class TimeoutPolicyTest {

  [Fact]
  public async Task CallWithTimeout() {
    IAsyncQuery<int, int> fastQuery = AsyncExecutable
      .Create(async (int num, CancellationToken token) => {
        await Task.Delay(10, token);
        return num * 2;
      })
      .WithPolicy(TimeoutPolicy)
      .AsQuery();

    Assert.Equal(20, await fastQuery.Send(10));

    IAsyncQuery<int, int> slowQuery = AsyncExecutable
      .Create(async (int num, CancellationToken token) => {
        await Task.Delay(1000, token);
        return num + 10;
      })
      .WithPolicy(TimeoutPolicy)
      .AsQuery();

    await Assert.ThrowsAsync<TimeoutException>(async () => await slowQuery.Send(10));
    return;

    void TimeoutPolicy(AsyncPolicyBuilder<int, int> builder) {
      builder.Timeout(TimeSpan.FromMilliseconds(500));
    }
  }

}