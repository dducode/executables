using Executables.Validation;
using JetBrains.Annotations;
using static Executables.Validation.Validator;

namespace Executables.Tests.Validation;

[TestSubject(typeof(Validator<>))]
public class CompositeValidatorsTest {

  [Theory]
  [InlineData(2, true)]
  [InlineData(3, false)]
  [InlineData(-2, false)]
  public void RangeTest(int value, bool expected) {
    Validator<int> validator = InRange(0, 3);
    Assert.Equal(expected, validator.IsValid(value));
  }

  [Theory]
  [InlineData(0, 3, 5, -1, false)]
  [InlineData(0, 3, 5, 1, true)]
  [InlineData(0, 3, 5, 3, false)]
  [InlineData(0, 3, 5, 4, true)]
  [InlineData(0, 3, 5, 5, false)]
  public void RangeExcludingValueTest(int min, int forbidden, int max, int value, bool expected) {
    Validator<int> validator = InRange(min, max).And(NotEqual(forbidden));
    Assert.Equal(expected, validator.IsValid(value));
  }

  [Theory]
  [InlineData(0, 3, 42, 0, true)]
  [InlineData(0, 3, 42, 2, true)]
  [InlineData(0, 3, 42, 42, true)]
  [InlineData(0, 3, 42, 25, false)]
  public void SpecialRangeTest(int min, int max, int special, int value, bool expected) {
    Validator<int> validator = InRange(min, max).Or(Equal(special));
    Assert.Equal(expected, validator.IsValid(value));
  }

  [Theory]
  [InlineData(0, 3, 7, 10, 1, true)]
  [InlineData(0, 3, 7, 10, 5, false)]
  [InlineData(0, 3, 7, 10, 8, true)]
  [InlineData(0, 3, 7, 10, 11, false)]
  public void RangeExcludingOtherRangeTest(int min, int innerMin, int innerMax, int max, int value, bool expected) {
    Validator<int> validator = InRange(min, max).And(OutRange(innerMin, innerMax));
    Assert.Equal(expected, validator.IsValid(value));
  }

  [Fact]
  public void EquivalentValidators() {
    Validator<int> v1 = InRange(0, 10);
    Validator<int> v2 = MoreThan(0).Or(Equal(0)).And(LessThan(10));

    foreach (int num in Enumerable.Range(-2, 14))
      Assert.Equal(v1.IsValid(num), v2.IsValid(num));
  }

}