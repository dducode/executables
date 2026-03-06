using AutoFixture;
using Interactions.Transformation.Filtering;
using JetBrains.Annotations;
using static Interactions.Validation.Validator;

namespace Interactions.Tests.Transformation.Filtering;

[TestSubject(typeof(ConditionalFilter<>))]
public class ConditionalFilterTest {

  [Fact]
  public void FilterStringsByLengthTest() {
    var fixture = new Fixture();
    var random = new Random(42);
    var list = new List<string>(fixture.CreateMany<string>(random.Next(10, 30)));
    Filter<string> filter = Filter.Where(StringLength(LessThan(5)));

    int expected = list.Count(s => s.Length < 5);
    IEnumerable<string> result = filter.Transform(list);

    Assert.Equal(expected, result.Count());
  }

}