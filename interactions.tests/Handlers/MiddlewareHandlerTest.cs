using Interactions.Core;
using Interactions.Core.Extensions;
using Interactions.Core.Queries;
using Interactions.Pipelines;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Interactions.Tests.Handlers;

[TestSubject(typeof(MiddlewareHandler<,,,>))]
public class MiddlewareHandlerTest(ITestOutputHelper testOutputHelper) {

  [Fact]
  public void NestedInvocationTest() {
    var firstStarted = false;
    var secondStarted = false;

    var secondEnd = false;
    var thirdEnd = false;

    var query = new Query<Unit, Unit>();
    using IDisposable handle = query.Handle(Pipeline
      .Use(next => {
        testOutputHelper.WriteLine("Start first");
        firstStarted = true;
        next.Invoke();
        Assert.True(secondEnd);
        testOutputHelper.WriteLine("End first");
      })
      .Use(next => {
        Assert.True(firstStarted);
        testOutputHelper.WriteLine("Start second");
        secondStarted = true;
        next.Invoke();
        Assert.True(thirdEnd);
        testOutputHelper.WriteLine("End second");
        secondEnd = true;
      })
      .End(Handler.Create(() => {
        Assert.True(secondStarted);
        testOutputHelper.WriteLine("Finish");
        thirdEnd = true;
      }))
    );

    query.Execute();
  }

  [Theory]
  [InlineData("00:05:00", "00:05:05", 5)]
  [InlineData("00:00:10", "00:00:00", -10)]
  public void PipelineWithDifferentTypesTest(string input, string expected, int addedSeconds) {
    var query = new Query<string, string>();
    using IDisposable handle = query.Handle(Pipeline<string, string>
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
      .End(Handler.Create((double seconds) => (long)(seconds + addedSeconds)))
    );

    Assert.Equal(expected, query.Execute(input));
  }

}