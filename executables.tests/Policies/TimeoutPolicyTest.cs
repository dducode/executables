using Executables.Core.Policies;
using Executables.Policies;
using JetBrains.Annotations;

namespace Executables.Tests.Policies;

[TestSubject(typeof(TimeoutPolicy<,>))]
public class TimeoutPolicyTest {

  [Fact]
  public async Task CallWithTimeout() {
    IAsyncExecutor<int, int> fastQuery = AsyncExecutable
      .Create(async (int num, CancellationToken token) => {
        await Task.Delay(10, token);
        return num * 2;
      })
      .GetExecutor()
      .WithPolicy(TimeoutPolicy);

    Assert.Equal(20, await fastQuery.Execute(10));

    IAsyncExecutor<int, int> slowQuery = AsyncExecutable
      .Create(async (int num, CancellationToken token) => {
        await Task.Delay(1000, token);
        return num + 10;
      })
      .GetExecutor()
      .WithPolicy(TimeoutPolicy);

    await Assert.ThrowsAsync<TimeoutException>(async () => await slowQuery.Execute(10));
    return;

    void TimeoutPolicy(AsyncPolicyBuilder<int, int> builder) {
      builder.Timeout(TimeSpan.FromMilliseconds(100));
    }
  }

}