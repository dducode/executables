using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Linq;
using JetBrains.Annotations;

namespace Interactions.Tests.Linq;

[TestSubject(typeof(LinqExtensions))]
public class LinqExtensionsTest {

  [Theory]
  [InlineData("00:00:00", 0)]
  [InlineData("00:00:10", 10)]
  [InlineData("00:01:00", 60)]
  public void SelectExpression(string expected, int value) {
    IExecutable<int, string> executable =
      from time in Executable.Create((int seconds) => TimeSpan.FromSeconds(seconds))
      select time.ToString();

    IQuery<int, string> query = executable.AsQuery();
    Assert.Equal(expected, query.Send(value));
  }

}