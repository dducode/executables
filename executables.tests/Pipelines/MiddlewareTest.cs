using Executables.Context;
using Executables.Pipelines;
using JetBrains.Annotations;

namespace Executables.Tests.Pipelines;

[TestSubject(typeof(Middleware<,,,>))]
[TestSubject(typeof(AsyncMiddleware<,,,>))]
public class MiddlewareTest {

  [Fact]
  public void NestedMiddlewareInvocationTest() {
    var firstStarted = false;
    var secondStarted = false;

    var firstEnd = false;
    var secondEnd = false;
    var thirdEnd = false;

    IExecutor<Unit, Unit> executor = Pipeline
      .Use(next => {
        firstStarted = true;
        next.Invoke();
        Assert.True(secondEnd);
        firstEnd = true;
      })
      .Use(next => {
        Assert.True(firstStarted);
        secondStarted = true;
        next.Invoke();
        Assert.True(thirdEnd);
        secondEnd = true;
      })
      .End(Executable.Create(() => {
        Assert.True(secondStarted);
        thirdEnd = true;
      }));

    executor.Execute();
    Assert.True(firstEnd);
  }

  [Theory]
  [InlineData("00:05:00", "00:05:05", 5)]
  [InlineData("00:00:10", "00:00:00", -10)]
  public void PipelineWithDifferentTypesTest(string input, string expected, int addedSeconds) {
    IExecutor<string, string> executor = Pipeline
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
      .End(Executable.Create((double seconds) => (long)(seconds + addedSeconds)));

    Assert.Equal(expected, executor.Execute(input));
  }

  [Fact]
  public void NeverInvokeNextMiddleware() {
    IExecutor<int, int> executor = Pipeline
      .Use((int x, Action _) => x * 2)
      .End(Executable.Create(() => Assert.Fail()));

    Assert.Equal(20, executor.Execute(10));
  }

  [Theory]
  [InlineData("00:05:00", "00:05:05", 5)]
  [InlineData("00:00:10", "00:00:00", -10)]
  public void InvokePipelineWithContext(string input, string expected, int addedSeconds) {
    IExecutor<string, string> executor = Pipeline
      .Use((string time, Func<TimeSpan, TimeSpan> next) => {
        TimeSpan timeSpan = TimeSpan.Parse(time);
        TimeSpan result = next.Invoke(timeSpan);
        return result.ToString();
      })
      .End(Executable.Create((TimeSpan timeSpan) => timeSpan + TimeSpan.FromSeconds(ExecutableContext.Current.Get<int>(nameof(addedSeconds)))))
      .WithContext(context => context.Set(nameof(addedSeconds), addedSeconds));

    Assert.Equal(expected, executor.Execute(input));
  }

  [Theory]
  [InlineData("00:05:00", "00:05:05", 5)]
  [InlineData("00:00:10", "00:00:00", -10)]
  public async Task AsyncPipelineTest(string input, string expected, int addedSeconds) {
    IAsyncExecutor<string, string> executor = AsyncPipeline
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
      .End(AsyncExecutable.Create((double seconds, CancellationToken _) => ValueTask.FromResult((long)(seconds + addedSeconds))));

    Assert.Equal(expected, await executor.Execute(input));
  }

  [Fact]
  public async Task AsyncPipelineCancel() {
    var cts = new CancellationTokenSource();

    IAsyncExecutor<int, int> executor = AsyncPipeline
      .Use(async (int num, AsyncAction next, CancellationToken token) => {
        await Task.Delay(50, token);
        await cts.CancelAsync();
        var ex = await Assert.ThrowsAsync<TaskCanceledException>(async () => await next.Invoke(token));
        Assert.Equal(cts.Token, ex.CancellationToken);
        return num;
      })
      .End(AsyncExecutable.Create(async token => {
        await Task.Delay(10, token);
        Assert.Fail();
      }));

    Assert.Equal(10, await executor.Execute(10, cts.Token));
  }

}