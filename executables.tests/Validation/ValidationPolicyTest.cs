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
    IQuery<string, string> invokedQuery = query
      .WithPolicy(builder => builder.ValidateInput(Validator.NotEmptyString))
      .AsQuery();

    Assert.Throws<InvalidInputException>(() => invokedQuery.Send(string.Empty));
    Assert.Equal("response: request", invokedQuery.Send("request"));
  }

}