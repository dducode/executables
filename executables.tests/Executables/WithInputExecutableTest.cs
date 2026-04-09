using JetBrains.Annotations;

namespace Executables.Tests.Executables;

[TestSubject(typeof(Executable))]
public class WithInputExecutableTest {

  [Fact]
  public void SimpleAccumulate() {
    IQuery<int, string> query = Executable
      .Accumulate((int x) => x + x)
      .Accumulate((x, y) => x + y)
      .Accumulate((x, y, z) => x + y + z)
      .Merge((x, y, z, result) => $"{x}, {y}, {z}, {result}")
      .AsQuery();

    Assert.Equal("1, 2, 3, 6", query.Send(1));
  }

  [Fact]
  public void AccumulateWithDiffTypes() {
    IQuery<string, string> query = Executable
      .Accumulate((string input) => int.Parse(input))
      .Accumulate((_, seconds) => TimeSpan.FromSeconds(seconds))
      .Merge((input, seconds, time) => $"{input}, {seconds}, {time}")
      .AsQuery();

    Assert.Equal("1, 1, 00:00:01", query.Send("1"));
  }

}