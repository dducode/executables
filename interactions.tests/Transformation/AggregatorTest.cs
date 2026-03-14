using AutoFixture;
using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.LINQ;
using JetBrains.Annotations;

namespace Interactions.Tests.Transformation;

[TestSubject(typeof(Collections))]
public class AggregatorTest {

  [Fact]
  public void AddingNumbersTest() {
    var fixture = new Fixture();
    IQuery<IEnumerable<int>, int> query = Collections.Aggregate<int>((first, second) => first + second).AsQuery();
    var list = new List<int>(fixture.CreateMany<int>(10));
    int expected = list.Sum();

    Assert.Equal(expected, query.Send(list));
  }

}