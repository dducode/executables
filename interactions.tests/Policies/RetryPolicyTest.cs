using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Executables;
using Interactions.Policies;
using Interactions.RetryRules;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(RetryPolicy<,,>))]
public class RetryPolicyTest {

  [Fact]
  public async Task RetryToCallFailedQuery() {
    var failsCount = 3;

    IAsyncQuery<int, int> query = AsyncExecutable.Create(async (int num, CancellationToken token) => {
        if (failsCount-- > 0)
          throw new InvalidOperationException();
        await Task.Delay(50, token);
        failsCount = int.MaxValue;
        return num * 2;
      })
      .WithPolicy(builder => builder.Retry(RetryRule.ExponentialBackoff<InvalidOperationException>(TimeSpan.FromMilliseconds(10), 5)))
      .AsQuery();

    Assert.Equal(20, await query.Send(10));
    await Assert.ThrowsAsync<InvalidOperationException>(async () => await query.Send(1));
  }

}