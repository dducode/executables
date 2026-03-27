using AutoFixture;
using Interactions.Core.Validation;
using Interactions.Validation;
using JetBrains.Annotations;

namespace Interactions.Tests.Validation;

[TestSubject(typeof(CollectionCountValidator<>))]
public class CollectionCountValidatorTest {

  [Fact]
  public void NotEmptyCollectionTest() {
    var fixture = new Fixture();
    Validator<ICollection<int>> validator = Validator.NotEmptyCollection<int>();
    var list = new List<int>(fixture.CreateMany<int>(3));

    Assert.True(validator.IsValid(list));
    list.Clear();
    Assert.False(validator.IsValid(list));
    Assert.False(validator.IsValid(null!));
  }

}