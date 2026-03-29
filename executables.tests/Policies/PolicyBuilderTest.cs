using Executables.Policies;
using Executables.Validation;
using JetBrains.Annotations;

namespace Executables.Tests.Policies;

[TestSubject(typeof(PolicyBuilder<,>))]
public class PolicyBuilderTest {

  [Fact]
  public void ApplyValidationPolicyToQuery() {
    IQuery<int, int> query = Executable
      .Create((int x) => x * 2)
      .WithPolicy(builder => builder.ValidateInput(Validator.MoreThan(0)))
      .AsQuery();

    Assert.Equal(10, query.Send(5));
    Assert.Throws<InvalidInputException>(() => query.Send(-1));
  }

  [Fact]
  public void AppendPoliciesToQuery() {
    IQuery<int, int> query = Executable
      .Create((int x) => x * 2)
      .WithPolicy(builder => builder
        .ValidateInput(Validator.MoreThan(0))
        .ValidateOutput(Validator.LessThan(1000))
        .Fallback<InvalidOutputException>((x, _) => x)
      ).AsQuery();

    Assert.Equal(10, query.Send(5));
    Assert.Throws<InvalidInputException>(() => query.Send(-1));
    Assert.Equal(1000, query.Send(1000));
  }

}