using Interactions.Validation;
using JetBrains.Annotations;

namespace Interactions.Tests.Validation;

[TestSubject(typeof(OverrideMessageValidator<>))]
public class OverrideMessageValidatorTest {

  [Fact]
  public void OverrideMessageTest() {
    Validator<string> validator = Validator.NotEmptyString.OverrideMessage("Custom message");
    Assert.Equal("Custom message", validator.ErrorMessage);
  }

}