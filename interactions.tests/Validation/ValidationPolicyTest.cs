using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Operations;
using Interactions.Validation;
using JetBrains.Annotations;

namespace Interactions.Tests.Validation;

[TestSubject(typeof(ValidationPolicy<,>))]
public class ValidationPolicyTest {

  [Fact]
  public void ValidateRequestTest() {
    var query = new Query<string, string>();

    using IDisposable handle = query.Handle(Executable.Create((string request) => $"response: {request}").AsHandler());
    IQuery<string, string> invokedQuery = Policy
      .ValidateInput<string, string>(Validator.NotEmptyString)
      .Apply(query)
      .AsQuery();

    Assert.Throws<InvalidInputException>(() => invokedQuery.Send(string.Empty));
    Assert.Equal("response: request", invokedQuery.Send("request"));
  }

}