using Executables.Core.Policies;
using Executables.RetryRules;
using JetBrains.Annotations;

namespace Executables.Tests.Policies;

[TestSubject(typeof(RetryPolicy<,,>))]
public class RetryPolicyTest {

  [Fact]
  public async Task RetryToCallFailedQuery() {
    var failsCount = 3;

    IAsyncExecutor<int, int> executor = AsyncExecutable.Create(async (int num, CancellationToken token) => {
        if (failsCount-- > 0)
          throw new InvalidOperationException();
        await Task.Delay(50, token);
        failsCount = int.MaxValue;
        return num * 2;
      })
      .GetExecutor()
      .WithPolicy(builder => builder.Retry(RetryRule.ExponentialBackoff<InvalidOperationException>(TimeSpan.FromMilliseconds(10), 5)));

    Assert.Equal(20, await executor.Execute(10));
    await Assert.ThrowsAsync<InvalidOperationException>(async () => await executor.Execute(1));
  }

}