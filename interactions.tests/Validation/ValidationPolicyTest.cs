using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Policies;
using Interactions.Validation;
using JetBrains.Annotations;

namespace Interactions.Tests.Validation;

[TestSubject(typeof(ValidationPolicy<,>))]
public class ValidationPolicyTest {

  [Fact]
  public void ValidateRequestTest() {
    var query = new Query<string, string>();

    using IDisposable handle = query.Handle(Executable.Create((string request) => $"response: {request}").AsHandler());
    IExecutable<string, string> invokedQuery = Policy.Of<string>().ValidateInput(Validator.NotEmptyString).Apply(query);

    Assert.Throws<InvalidInputException>(() => invokedQuery.Execute(string.Empty));
    Assert.Equal("response: request", invokedQuery.Execute("request"));
  }

}