using Executables.Core.Executors;
using JetBrains.Annotations;

namespace Executables.Tests.Operators;

[TestSubject(typeof(ThrottleExecutor<>))]
public class ThrottleExecutorTest {

  [Fact]
  public void ThrottleQuery() {
    var executionCount = 0;
    IExecutor<Unit, Unit> executor = Executable
      .Create(delegate {
        executionCount++;
      })
      .GetExecutor()
      .Throttle(TimeSpan.FromMilliseconds(300));

    for (var i = 0; i < 5; i++) {
      executor.Execute();
      Thread.Sleep(50);
    }

    Assert.True(executionCount < 5);
  }

  [Fact]
  public async Task ThrottleQueryFromManyThreads() {
    var executionCount = 0;
    IExecutor<Unit, Unit> executor = Executable
      .Create(delegate {
        executionCount++;
      })
      .GetExecutor()
      .Throttle(TimeSpan.FromMilliseconds(300));

    var tasks = new List<Task>();

    for (var i = 0; i < 3; i++) {
      tasks.Add(Task.Run(() => {
        for (var j = 0; j < 3; j++) {
          executor.Execute();
          Thread.Sleep(50);
        }
      }));
    }

    await Task.WhenAll(tasks);
    Assert.True(executionCount < 9);
  }

}