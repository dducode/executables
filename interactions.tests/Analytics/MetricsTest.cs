using AutoFixture;
using Interactions.Analytics;
using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Operations;
using Interactions.Validation;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Interactions.Tests.Analytics;

[TestSubject(typeof(IMetrics<,>))]
public class MetricsTest(ITestOutputHelper outputHelper) {

  [Fact]
  public void RegularCall() {
    var fixture = new Fixture();
    BehaviorOperator<int, TimeSpan> metrics = ExecutionOperator.Metrics(Metrics.Create(
      (int input) => outputHelper.WriteLine($"{nameof(input)}: {input}"),
      (TimeSpan output) => outputHelper.WriteLine($"{nameof(output)}: {output}"),
      latency: latency => outputHelper.WriteLine($"{nameof(latency)}: {latency.Ticks}\n"))
    );

    IQuery<int, TimeSpan> query = Executable.Create((int seconds) => TimeSpan.FromSeconds(seconds))
      .Apply(metrics)
      .AsQuery();

    for (var i = 0; i < 5; i++)
      query.Send(fixture.Create<int>());
  }

  [Fact]
  public void CallWithException() {
    BehaviorOperator<int, TimeSpan> metrics = ExecutionOperator.Metrics(Metrics.Create(
      (int input) => outputHelper.WriteLine($"{nameof(input)}: {input}"),
      (TimeSpan output) => outputHelper.WriteLine($"{nameof(output)}: {output}"),
      error => outputHelper.WriteLine($"{nameof(error)}: {error}"),
      latency => outputHelper.WriteLine($"{nameof(latency)}: {latency.Ticks}\n"))
    );

    IQuery<int, TimeSpan> query = Executable.Create((int seconds) => TimeSpan.FromSeconds(seconds))
      .Apply(Policy.ValidateInput<int, TimeSpan>(Validator.MoreThanZero))
      .Apply(metrics)
      .AsQuery();

    query.Send(10);
    Assert.Throws<InvalidInputException>(() => query.Send(-10));
  }

}