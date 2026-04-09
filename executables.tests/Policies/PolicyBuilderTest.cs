using Executables.Policies;
using Executables.Validation;
using JetBrains.Annotations;

namespace Executables.Tests.Policies;

[TestSubject(typeof(PolicyBuilder<,>))]
public class PolicyBuilderTest {

  [Fact]
  public void ApplyValidationPolicyToQuery() {
    IExecutor<int, int> executor = Executable
      .Create((int x) => x * 2)
      .GetExecutor()
      .WithPolicy(builder => builder.ValidateInput(Validator.MoreThan(0)));

    Assert.Equal(10, executor.Execute(5));
    Assert.Throws<InvalidInputException>(() => executor.Execute(-1));
  }

  [Fact]
  public void AppendPoliciesToQuery() {
    IExecutor<int, int> executor = Executable
      .Create((int x) => x * 2)
      .GetExecutor()
      .WithPolicy(builder => builder
        .ValidateInput(Validator.MoreThan(0))
        .ValidateOutput(Validator.LessThan(1000))
        .Fallback<InvalidOutputException>((x, _) => x)
      );

    Assert.Equal(10, executor.Execute(5));
    Assert.Throws<InvalidInputException>(() => executor.Execute(-1));
    Assert.Equal(1000, executor.Execute(1000));
  }

}