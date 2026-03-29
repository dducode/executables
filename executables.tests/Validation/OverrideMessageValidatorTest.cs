using Executables.Core.Validation;
using Executables.Validation;
using JetBrains.Annotations;

namespace Executables.Tests.Validation;

[TestSubject(typeof(OverrideMessageValidator<>))]
public class OverrideMessageValidatorTest {

  [Fact]
  public void OverrideMessageTest() {
    Validator<string> validator = Validator.NotEmptyString.OverrideMessage("Custom message");
    Assert.Equal("Custom message", validator.ErrorMessage);
  }

}