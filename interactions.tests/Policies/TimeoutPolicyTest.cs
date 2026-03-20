using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Operations;
using Interactions.Policies;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(TimeoutPolicy<,>))]
public class TimeoutPolicyTest {

  [Fact]
  public async Task CallWithTimeout() {
    AsyncPolicy<int, int> timeout = AsyncPolicy.Timeout<int>(TimeSpan.FromMilliseconds(500));

    IAsyncQuery<int, int> fastQuery = AsyncExecutable
      .Create(async (int num, CancellationToken token) => {
        await Task.Delay(10, token);
        return num * 2;
      })
      .Apply(timeout)
      .AsQuery();

    Assert.Equal(20, await fastQuery.Send(10));

    IAsyncQuery<int, int> slowQuery = AsyncExecutable
      .Create(async (int num, CancellationToken token) => {
        await Task.Delay(1000, token);
        return num + 10;
      })
      .Apply(timeout)
      .AsQuery();

    await Assert.ThrowsAsync<TimeoutException>(async () => await slowQuery.Send(10));
  }

}