using Executables.Core.Validation;
using Executables.Validation;
using JetBrains.Annotations;
using static Executables.Validation.Validator;

namespace Executables.Tests.Validation;

[TestSubject(typeof(StringLengthValidator))]
public class StringLengthValidatorTest {

  [Theory]
  [InlineData("", false)]
  [InlineData(null, false)]
  [InlineData("test", true)]
  public void NotEmptyStringTest(string? value, bool expected) {
    Validator<string> validator = NotEmptyString;
    Assert.Equal(expected, validator.IsValid(value!));
  }

  [Theory]
  [InlineData("", false)]
  [InlineData("test", true)]
  [InlineData("long_test", false)]
  public void StringLengthTest(string value, bool expected) {
    Validator<string> validator = StringLength(InRange(1, 5));
    Assert.Equal(expected, validator.IsValid(value));
  }

}