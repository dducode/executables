using JetBrains.Annotations;

namespace Executables.Tests;

[TestSubject(typeof(ExecutableExtensions))]
public class MapTest {

  [Theory]
  [InlineData("20", "10")]
  [InlineData("30", "15")]
  public void Map(string expected, string value) {
    IQuery<string, string> query = Executable
      .Create((int x) => x * 2)
      .Map((string s) => int.Parse(s), x => x.ToString())
      .AsQuery();

    Assert.Equal(expected, query.Send(value));
  }

  [Theory]
  [InlineData(20, 10)]
  [InlineData(30, 15)]
  public void IdentityMap(int expected, int value) {
    IQuery<int, int> query = Executable
      .Create((int x) => x * 2)
      .Map((int t1) => t1, t2 => t2)
      .AsQuery();

    Assert.Equal(expected, query.Send(value));
  }

}