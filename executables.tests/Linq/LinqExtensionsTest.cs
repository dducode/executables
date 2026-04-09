using Executables.Linq;
using JetBrains.Annotations;

namespace Executables.Tests.Linq;

[TestSubject(typeof(LinqExtensions))]
public class LinqExtensionsTest {

  [Theory]
  [InlineData("00:00:00", 0)]
  [InlineData("00:00:10", 10)]
  [InlineData("None", -1)]
  [InlineData("None", 60)]
  public void SelectExpression(string expected, int value) {
    IExecutable<int, string> executable =
      from time in Executable.Create((int seconds) => TimeSpan.FromSeconds(seconds))
      where time >= TimeSpan.Zero
      where time < TimeSpan.FromSeconds(60)
      select time.ToString();

    IQuery<int, string> query = executable.AsQuery();
    Assert.Equal(expected, query.Send(value));
  }

}