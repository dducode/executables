using Interactions.Core;
using Interactions.Linq;
using JetBrains.Annotations;

namespace Interactions.Tests.Executables;

[TestSubject(typeof(LinqExecutableExtensions))]
public class LinqExecutableExtensionsTest {

  [Theory]
  [InlineData("00:00:00", 0)]
  [InlineData("00:00:10", 10)]
  [InlineData("00:01:00", 60)]
  public void SelectExpression(string expected, int value) {
    IExecutable<int, string> executable =
      from x in Executable.Create((int seconds) => TimeSpan.FromSeconds(seconds))
      select x.ToString();

    Assert.Equal(expected, executable.Execute(value));
  }

  [Theory]
  [InlineData("00:00:00", 0)]
  [InlineData("00:00:10", 10)]
  [InlineData("00:01:00", 60)]
  public async Task AsyncSelectExpression(string expected, int value) {
    IAsyncExecutable<int, string> executable =
      from x in AsyncExecutable.Create((int seconds, CancellationToken _) => ValueTask.FromResult(TimeSpan.FromSeconds(seconds)))
      select x.ToString();

    Assert.Equal(expected, await executable.Execute(value));
  }

  [Theory]
  [InlineData("00:00:00", 0)]
  [InlineData("00:00:10", 10)]
  [InlineData("00:01:00", 60)]
  public void SelectManyExpression(string expected, int value) {
    IExecutable<int, string> executable =
      from x in Executable.Create((int seconds) => TimeSpan.FromSeconds(seconds))
      from y in Executable.Create((TimeSpan time) => time.ToString())
      select y;

    Assert.Equal(expected, executable.Execute(value));
  }

  [Theory]
  [InlineData("00:00:00", 0)]
  [InlineData("00:00:10", 10)]
  [InlineData("00:01:00", 60)]
  public async Task AsyncSelectManyExpression(string expected, int value) {
    IAsyncExecutable<int, string> executable =
      from x in AsyncExecutable.Create((int seconds, CancellationToken _) => ValueTask.FromResult(TimeSpan.FromSeconds(seconds)))
      from y in AsyncExecutable.Create((TimeSpan time, CancellationToken _) => ValueTask.FromResult(time.ToString()))
      select y;

    Assert.Equal(expected, await executable.Execute(value));
  }

  [Fact]
  public void WhereExpression() {
    IExecutable<int, string> executable =
      from x in Executable.Create((int seconds) => TimeSpan.FromSeconds(seconds))
      where x >= TimeSpan.Zero
      select x.ToString();

    Assert.Equal("00:00:10", executable.Execute(10));
    Assert.Equal("00:00:00", executable.Execute(0));
    Assert.Throws<InvalidOperationException>(() => executable.Execute(-10));
  }

  [Fact]
  public async Task AsyncWhereExpression() {
    IAsyncExecutable<int, string> executable =
      from x in AsyncExecutable.Create((int seconds, CancellationToken _) => ValueTask.FromResult(TimeSpan.FromSeconds(seconds)))
      where x >= TimeSpan.Zero
      select x.ToString();

    Assert.Equal("00:00:10", await executable.Execute(10));
    Assert.Equal("00:00:00", await executable.Execute(0));
    await Assert.ThrowsAsync<InvalidOperationException>(async () => await executable.Execute(-10));
  }

}