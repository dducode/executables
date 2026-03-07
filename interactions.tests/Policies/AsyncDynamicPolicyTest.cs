using Interactions.Core;
using Interactions.Policies;
using Interactions.Validation;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(AsyncDynamicPolicy<,>))]
public class AsyncDynamicPolicyTest {

  [Fact]
  public async Task SwitchPolicyByCondition() {
    var validate = false;
    AsyncPolicy<int, int> policy = AsyncPolicy<int, int>.Optional(() => validate, AsyncPolicy<int, int>.ValidateInput(Validator.MoreThan(0)));

    Assert.Equal(1, await policy.Invoke(-1, AsyncExecutable.Create((int num, CancellationToken _) => new ValueTask<int>(num * -1)), CancellationToken.None));
    validate = true;
    await Assert.ThrowsAsync<InvalidInputException>(async () => {
      await policy.Invoke(-1, AsyncExecutable.Create((int num, CancellationToken _) => new ValueTask<int>(num * -1)), CancellationToken.None);
    });
  }

}