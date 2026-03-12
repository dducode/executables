using Interactions.Core;
using Interactions.Filtering;
using JetBrains.Annotations;

namespace Interactions.Tests.Transformation.Filtering;

[TestSubject(typeof(IExecutable<,>))]
public class UniqueFilterTest {

  [Fact]
  public void SelectUniqueNumbersTest() {
    var list = new List<int> { 1, 1, 2, 3, 3 };
    IFilter<int> filter = Collections.Distinct<int>();

    Assert.Equal(3, filter.Execute(list).Count());
  }

}