using Executables.Linq;
using Executables.Validation;
using JetBrains.Annotations;

namespace Executables.Tests.Transformation.Filtering;

[TestSubject(typeof(Collections))]
public class CompositeFilterTest {

  [Fact]
  public void SelectSingleNumberTest() {
    var list = new List<int> { -1, 0, 2, 2, 2, 3, 4, 10, 10, 26, 30 };
    IQuery<IEnumerable<int>, IEnumerable<int>> query = Collections.Skip<int>(2).Distinct().Where(Validator.InRange(5, 20)).AsQuery();

    Assert.Single(query.Send(list));
  }

}