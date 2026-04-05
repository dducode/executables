using AutoFixture;
using Executables.Analytics;
using Executables.Policies;
using Executables.Validation;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Executables.Tests.Analytics;

[TestSubject(typeof(IMetrics<,>))]
public class MetricsTest(ITestOutputHelper outputHelper) {

  [Fact]
  public void RegularCall() {
    var fixture = new Fixture();

    IExecutor<int, TimeSpan> query = Executable
      .Create((int seconds) => TimeSpan.FromSeconds(seconds))
      .GetExecutor()
      .Metrics(Metrics.Create(
        (int input) => outputHelper.WriteLine($"{nameof(input)}: {input}"),
        (TimeSpan output) => outputHelper.WriteLine($"{nameof(output)}: {output}"),
        latency: latency => outputHelper.WriteLine($"{nameof(latency)}: {latency.Ticks}\n"))
      );

    for (var i = 0; i < 5; i++)
      query.Execute(fixture.Create<int>());
  }

  [Fact]
  public void CallWithException() {
    IExecutor<int, TimeSpan> executor = Executable
      .Create((int seconds) => TimeSpan.FromSeconds(seconds))
      .GetExecutor()
      .WithPolicy(builder => builder.ValidateInput(Validator.MoreThanZero))
      .Metrics(Metrics.Create(
        (int input) => outputHelper.WriteLine($"{nameof(input)}: {input}"),
        (TimeSpan output) => outputHelper.WriteLine($"{nameof(output)}: {output}"),
        error => outputHelper.WriteLine($"{nameof(error)}: {error}"),
        latency => outputHelper.WriteLine($"{nameof(latency)}: {latency.Ticks}\n"))
      );

    executor.Execute(10);
    Assert.Throws<InvalidInputException>(() => executor.Execute(-10));
  }

}