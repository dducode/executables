using AutoFixture;
using Executables.Linq;
using JetBrains.Annotations;
using static Executables.Validation.Validator;

namespace Executables.Tests.Transformation.Filtering;

[TestSubject(typeof(Collections))]
public class ConditionalFilterTest {

  [Fact]
  public void FilterStringsByLengthTest() {
    var fixture = new Fixture();
    var random = new Random(42);
    var list = new List<string>(fixture.CreateMany<string>(random.Next(10, 30)));
    IQuery<IEnumerable<string>, IEnumerable<string>> query = Collections.Where(StringLength(LessThan(5))).AsQuery();

    int expected = list.Count(s => s.Length < 5);
    IEnumerable<string> result = query.Send(list);

    Assert.Equal(expected, result.Count());
  }

}