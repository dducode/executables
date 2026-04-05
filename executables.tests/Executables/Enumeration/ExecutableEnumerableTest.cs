using AutoFixture;
using Executables.Enumeration;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Executables.Tests.Executables.Enumeration;

[TestSubject(typeof(ExecutableEnumerable<,>))]
public class ExecutableEnumerableTest(ITestOutputHelper output) {

  [Fact]
  public void ExecuteWithListArgs() {
    var fixture = new Fixture();
    var list = new List<int>(5);

    for (var i = 0; i < list.Capacity; i++)
      list.Add(fixture.Create<int>());

    IExecutor<int, TimeSpan> query = Executable
      .Create((int seconds) => TimeSpan.FromSeconds(seconds))
      .GetExecutor();

    foreach (TimeSpan time in query.ForEach(list))
      output.WriteLine($"Time: {time}");
  }

  [Fact]
  public void ParallelListExecution() {
    var fixture = new Fixture();
    var list = new List<int>(5);

    for (var i = 0; i < list.Capacity; i++)
      list.Add(fixture.Create<int>());

    IExecutor<int, TimeSpan> query = Executable
      .Create((int seconds) => TimeSpan.FromSeconds(seconds))
      .GetExecutor();

    Parallel.ForEach(query.ForEach(list), time => output.WriteLine($"Time: {time}"));
  }

  [Fact]
  public void ExecuteWithArrayArgs() {
    var fixture = new Fixture();
    var array = new int[5];

    for (var i = 0; i < array.Length; i++)
      array[i] = fixture.Create<int>();

    IExecutor<int, TimeSpan> query = Executable
      .Create((int seconds) => TimeSpan.FromSeconds(seconds))
      .GetExecutor();

    foreach (TimeSpan time in query.ForEach(array))
      output.WriteLine($"Time: {time}");
  }

  [Fact]
  public void ParallelArrayExecution() {
    var fixture = new Fixture();
    var array = new int[5];

    for (var i = 0; i < array.Length; i++)
      array[i] = fixture.Create<int>();

    IExecutor<int, TimeSpan> query = Executable
      .Create((int seconds) => TimeSpan.FromSeconds(seconds))
      .GetExecutor();

    Parallel.ForEach(query.ForEach(array), time => output.WriteLine($"Time: {time}"));
  }

}