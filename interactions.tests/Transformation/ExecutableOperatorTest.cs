using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Executables;
using Interactions.Operations;
using JetBrains.Annotations;
using Xunit.Abstractions;
using Interactions.Pipelines;
using static Interactions.Validator;

namespace Interactions.Tests.Transformation;

[TestSubject(typeof(ExecutableOperator<,,,>))]
public class ExecutableOperatorTest(ITestOutputHelper output) {

  [Fact]
  public void ParseNumberTest() {
    IExecutable<string, string> executable = Executable.Create((int num) => num + num).Parse(int.Parse);

    Assert.Equal("84", executable.Execute("42"));
    Assert.Throws<FormatException>(() => executable.Execute("not-a-number"));
    Assert.Throws<OverflowException>(() => executable.Execute("1251328907421983752137032985702938"));
  }

  [Fact]
  public void FilterListTest() {
    IExecutable<IEnumerable<string>, string> filter = Executable
      .Identity<string>()
      .InMap(Collections.First<string>())
      .InFilter(Collections.Where(StringLength(MoreThan(2))));

    Assert.Equal("input", filter.Execute(new List<string> { string.Empty, "10", "input" }));
  }

  [Fact]
  public void NestedMiddlewareInvocationTest() {
    var firstStarted = false;
    var secondStarted = false;

    var secondEnd = false;
    var thirdEnd = false;

    IExecutable<Unit> executable = Pipeline
      .Use(next => {
        output.WriteLine("Start first");
        firstStarted = true;
        next.Invoke();
        Assert.True(secondEnd);
        output.WriteLine("End first");
      })
      .Use(next => {
        Assert.True(firstStarted);
        output.WriteLine("Start second");
        secondStarted = true;
        next.Invoke();
        Assert.True(thirdEnd);
        output.WriteLine("End second");
        secondEnd = true;
      })
      .End(() => {
        Assert.True(secondStarted);
        output.WriteLine("Finish");
        thirdEnd = true;
      });

    executable.Execute();
  }

  [Theory]
  [InlineData("00:05:00", "00:05:05", 5)]
  [InlineData("00:00:10", "00:00:00", -10)]
  public void PipelineWithDifferentTypesTest(string input, string expected, int addedSeconds) {
    IExecutable<string, string> executable = Pipeline<string, string>
      .Use((string time, Func<TimeSpan, TimeSpan> next) => {
        TimeSpan timeSpan = TimeSpan.Parse(time);
        TimeSpan result = next.Invoke(timeSpan);
        return result.ToString();
      })
      .Use((TimeSpan time, Func<double, long> next) => {
        double seconds = time.TotalSeconds;
        long result = next.Invoke(seconds);
        return TimeSpan.FromSeconds(result);
      })
      .End(seconds => (long)(seconds + addedSeconds));

    Assert.Equal(expected, executable.Execute(input));
  }

}