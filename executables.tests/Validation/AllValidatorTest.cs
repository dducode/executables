using AutoFixture;
using Executables.Core.Validation;
using Executables.Validation;
using JetBrains.Annotations;
using static Executables.Validation.Validator;

namespace Executables.Tests.Validation;

[TestSubject(typeof(AllValidator<>))]
public class AllValidatorTest {

  [Fact]
  public void AllItemsIsValidTest() {
    var fixture = new Fixture();
    Validator<IEnumerable<string>> validator = All(NotEmptyString);
    var list = new List<string>(fixture.CreateMany<string>(3));
    Assert.True(validator.IsValid(list));

    list.Add(string.Empty);
    Assert.False(validator.IsValid(list));
  }

}