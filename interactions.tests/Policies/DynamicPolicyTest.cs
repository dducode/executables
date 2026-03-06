using Interactions.Core;
using Interactions.Policies;
using Interactions.Validation;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(DynamicPolicy<,>))]
public class DynamicPolicyTest {

  [Fact]
  public void SwitchPolicyByCondition() {
    var validate = false;
    Policy<int, int> policy = Policy<int, int>.Optional(() => validate, Policy<int, int>.ValidateInput(Validator.MoreThan(0)));

    Assert.Equal(1, policy.Execute(-1, Executable.Create((int num) => num * -1)));
    validate = true;
    Assert.Throws<InvalidInputException>(() => policy.Execute(-1, Executable.Create((int num) => num)));
  }

}