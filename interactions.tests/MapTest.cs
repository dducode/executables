using Interactions.Operations;
using JetBrains.Annotations;

namespace Interactions.Tests;

[TestSubject(typeof(Map<,,,>))]
public class MapTest {

  [Theory]
  [InlineData("20", "10")]
  [InlineData("30", "15")]
  public void Map(string expected, string value) {
    IQuery<string, string> query = Executable
      .Create((int x) => x * 2)
      .Map(Executable.Create((string s) => int.Parse(s)), Executable.Create((int x) => x.ToString()))
      .AsQuery();

    Assert.Equal(expected, query.Send(value));
  }

  [Theory]
  [InlineData(20, "10")]
  [InlineData(30, "15")]
  public void InMap(int expected, string value) {
    IQuery<string, int> query = Executable
      .Create((int x) => x * 2)
      .InMap(Executable.Create((string s) => int.Parse(s)))
      .AsQuery();

    Assert.Equal(expected, query.Send(value));
  }

  [Theory]
  [InlineData("20", 10)]
  [InlineData("30", 15)]
  public void OutMap(string expected, int value) {
    IQuery<int, string> query = Executable
      .Create((int x) => x * 2)
      .OutMap(Executable.Create((int x) => x.ToString()))
      .AsQuery();

    Assert.Equal(expected, query.Send(value));
  }

  [Theory]
  [InlineData(20, 10)]
  [InlineData(30, 15)]
  public void IdentityMap(int expected, int value) {
    IQuery<int, int> query = Executable
      .Create((int x) => x * 2)
      .Map(Executable.Identity<int>(), Executable.Identity<int>())
      .AsQuery();

    Assert.Equal(expected, query.Send(value));
  }

}