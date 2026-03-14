using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Core.Queries;
using Interactions.Operations;
using JetBrains.Annotations;
using Xunit.Abstractions;
using Interactions.Pipelines;

namespace Interactions.Tests.Transformation;

[TestSubject(typeof(ExecutableOperator<,,,>))]
public class ExecutableOperatorTest(ITestOutputHelper output) {

  [Fact]
  public void NestedMiddlewareInvocationTest() {
    var firstStarted = false;
    var secondStarted = false;

    var secondEnd = false;
    var thirdEnd = false;

    IQuery<Unit, Unit> query = Pipeline
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
      }).AsQuery();

    query.Send();
  }

  [Theory]
  [InlineData("00:05:00", "00:05:05", 5)]
  [InlineData("00:00:10", "00:00:00", -10)]
  public void PipelineWithDifferentTypesTest(string input, string expected, int addedSeconds) {
    IQuery<string, string> query = Pipeline<string, string>
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
      .End(seconds => (long)(seconds + addedSeconds))
      .AsQuery();

    Assert.Equal(expected, query.Send(input));
  }

}