using Interactions.Context;
using Interactions.Pipelines;
using Interactions.Queries;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Interactions.Tests.Pipelines;

[TestSubject(typeof(Middleware<,,,>))]
public class MiddlewareTest(ITestOutputHelper output) {

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

  [Fact]
  public void NeverInvokeNextMiddleware() {
    IQuery<int, int> query = Pipeline<int, int>
      .Use((x, _) => x * 2)
      .End(() => Assert.Fail())
      .AsQuery();

    Assert.Equal(20, query.Send(10));
  }

  [Theory]
  [InlineData("00:05:00", "00:05:05", 5)]
  [InlineData("00:00:10", "00:00:00", -10)]
  public void InvokePipelineWithContext(string input, string expected, int addedSeconds) {
    IQuery<string, string> query = Pipeline<string, string>
      .Use((string time, Func<TimeSpan, TimeSpan> next) => {
        TimeSpan timeSpan = TimeSpan.Parse(time);
        TimeSpan result = next.Invoke(timeSpan);
        return result.ToString();
      })
      .End(timeSpan => timeSpan + TimeSpan.FromSeconds(InteractionContext.Current.Get<int>(nameof(addedSeconds))))
      .WithContext(context => context.Set(nameof(addedSeconds), addedSeconds))
      .AsQuery();

    Assert.Equal(expected, query.Send(input));
  }

  [Theory]
  [InlineData("00:05:00", "00:05:05", 5)]
  [InlineData("00:00:10", "00:00:00", -10)]
  public async Task AsyncPipelineTest(string input, string expected, int addedSeconds) {
    IAsyncQuery<string, string> query = AsyncPipeline<string, string>
      .Use(async (string time, AsyncFunc<TimeSpan, TimeSpan> next, CancellationToken token) => {
        TimeSpan timeSpan = TimeSpan.Parse(time);
        await Task.Delay(50, token);
        TimeSpan result = await next.Invoke(timeSpan, token);
        return result.ToString();
      })
      .Use(async (TimeSpan time, AsyncFunc<double, long> next, CancellationToken token) => {
        double seconds = time.TotalSeconds;
        await Task.Delay(10, token);
        long result = await next.Invoke(seconds, token);
        return TimeSpan.FromSeconds(result);
      })
      .End((seconds, _) => ValueTask.FromResult((long)(seconds + addedSeconds)))
      .AsQuery();

    Assert.Equal(expected, await query.Send(input));
  }

  [Fact]
  public async Task AsyncPipelineCancel() {
    var cts = new CancellationTokenSource();

    IAsyncQuery<int, int> query = AsyncPipeline<int, int>
      .Use(async (num, next, token) => {
        await Task.Delay(50, token);
        await cts.CancelAsync();
        var ex = await Assert.ThrowsAsync<TaskCanceledException>(async () => await next.Invoke(token));
        Assert.Equal(cts.Token, ex.CancellationToken);
        return num;
      })
      .End(async token => {
        await Task.Delay(10, token);
        Assert.Fail();
      })
      .AsQuery();

    Assert.Equal(10, await query.Send(10, cts.Token));
  }

}