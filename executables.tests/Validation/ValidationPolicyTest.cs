using Executables.Core.Policies;
using Executables.Policies;
using Executables.Validation;
using JetBrains.Annotations;

namespace Executables.Tests.Validation;

[TestSubject(typeof(ValidationPolicy<,>))]
public class ValidationPolicyTest {

  [Fact]
  public void ValidateRequestTest() {
    var query = new Query<string, string>();

    using IDisposable handle = query.Handle(Executable.Create((string request) => $"response: {request}").AsHandler());
    IExecutor<string, string> executor = query.GetExecutor()
      .WithPolicy(builder => builder.ValidateInput(Validator.NotEmptyString));

    Assert.Throws<InvalidInputException>(() => executor.Execute(string.Empty));
    Assert.Equal("response: request", executor.Execute("request"));
  }

}