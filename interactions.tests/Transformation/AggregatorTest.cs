using AutoFixture;
using Interactions.Core;
using JetBrains.Annotations;

namespace Interactions.Tests.Transformation;

[TestSubject(typeof(IExecutable<,>))]
public class AggregatorTest {

  [Fact]
  public void AddingNumbersTest() {
    var fixture = new Fixture();
    IExecutable<IEnumerable<int>, int> transformer = Transformer.Aggregate<int>((first, second) => first + second);
    var list = new List<int>(fixture.CreateMany<int>(10));
    int expected = list.Sum();

    Assert.Equal(expected, transformer.Execute(list));
  }

}