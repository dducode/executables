using Interactions.Core;
using Interactions.Filtering;
using JetBrains.Annotations;

namespace Interactions.Tests.Transformation.Filtering;

[TestSubject(typeof(IExecutable<,>))]
public class CompositeFilterTest {

  [Fact]
  public void SelectSingleNumberTest() {
    var list = new List<int> { -1, 0, 2, 2, 2, 3, 4, 10, 10, 26, 30 };
    IFilter<int> filter = Filter.Skip<int>(2).Distinct().Where(Validator.InRange(5, 20));

    Assert.Single(filter.Execute(list));
  }

}