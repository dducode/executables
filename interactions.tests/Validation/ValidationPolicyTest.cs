using Interactions.Core;
using Interactions.Core.Queries;
using Interactions.Extensions;
using Interactions.Policies;
using Interactions.Validation;
using JetBrains.Annotations;

namespace Interactions.Tests.Validation;

[TestSubject(typeof(ValidationPolicy<,>))]
public class ValidationPolicyTest {

  [Fact]
  public void ValidateRequestTest() {
    var query = new Query<string, string>();

    using IDisposable handle = query.Handle(Handler.Create((string request) => $"response: {request}"));
    IExecutable<string, string> invokedQuery = query.WithPolicy(Policy<string, string>.ValidateInput(Validator.NotEmptyString));

    Assert.Throws<InvalidInputException>(() => invokedQuery.Execute(string.Empty));
    Assert.Equal("response: request", invokedQuery.Execute("request"));
  }

}