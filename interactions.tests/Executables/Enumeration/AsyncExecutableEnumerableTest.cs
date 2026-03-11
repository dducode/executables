using AutoFixture;
using Interactions.Core;
using Interactions.Executables.Enumeration;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Interactions.Tests.Executables.Enumeration;

[TestSubject(typeof(AsyncExecutableEnumerable<,>))]
public class AsyncExecutableEnumerableTest(ITestOutputHelper output) {

  [Fact]
  public async Task ExecuteWithListArgs() {
    var fixture = new Fixture();
    var list = new List<int>();

    for (var i = 0; i < 5; i++)
      list.Add(fixture.Create<int>());

    IAsyncExecutable<int, TimeSpan> executable = AsyncExecutable.Create((int num, CancellationToken _) => ValueTask.FromResult(TimeSpan.FromSeconds(num)));
    await foreach (TimeSpan time in executable.ForEach(GetItems()))
      output.WriteLine($"Time: {time}");
    return;

    async IAsyncEnumerable<int> GetItems() {
      foreach (int num in list) {
        await Task.Yield();
        yield return num;
      }
    }
  }

}