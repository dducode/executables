using Executables.Linq;
using JetBrains.Annotations;

namespace Executables.Tests.Transformation.Filtering;

[TestSubject(typeof(Collections))]
public class UniqueFilterTest {

  [Fact]
  public void SelectUniqueNumbersTest() {
    var list = new List<int> { 1, 1, 2, 3, 3 };
    IQuery<IEnumerable<int>, IEnumerable<int>> query = Collections.Distinct<int>().AsQuery();

    Assert.Equal(3, query.Send(list).Count());
  }

}