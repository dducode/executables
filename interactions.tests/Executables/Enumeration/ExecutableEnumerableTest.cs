using AutoFixture;
using Interactions.Core;
using Interactions.Executables.Enumeration;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Interactions.Tests.Executables.Enumeration;

[TestSubject(typeof(ExecutableEnumerable<,>))]
public class ExecutableEnumerableTest(ITestOutputHelper output) {

  [Fact]
  public void ExecuteWithListArgs() {
    var fixture = new Fixture();
    var list = new List<int>();

    for (var i = 0; i < 5; i++)
      list.Add(fixture.Create<int>());

    IExecutable<int, TimeSpan> executable = Executable.Create((int num) => TimeSpan.FromSeconds(num));
    foreach (TimeSpan time in executable.ForEach(list))
      output.WriteLine($"Time: {time}");
  }

}