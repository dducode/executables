using Executables.Core.Operators;
using Executables.Queries;
using JetBrains.Annotations;

namespace Executables.Tests.Operators;

[TestSubject(typeof(ThrottleOperator<>))]
public class ThrottleOperatorTest {

  [Fact]
  public void ThrottleQuery() {
    var executionCount = 0;
    IQuery<Unit, Unit> query = Executable
      .Create(delegate {
        executionCount++;
      })
      .Throttle(TimeSpan.FromMilliseconds(300))
      .AsQuery();

    for (var i = 0; i < 5; i++) {
      query.Send();
      Thread.Sleep(50);
    }

    Assert.True(executionCount < 5);
  }

  [Fact]
  public async Task ThrottleQueryFromManyThreads() {
    var executionCount = 0;
    IQuery<Unit, Unit> query = Executable
      .Create(delegate {
        executionCount++;
      })
      .Throttle(TimeSpan.FromMilliseconds(300))
      .AsQuery();

    var tasks = new List<Task>();

    for (var i = 0; i < 3; i++) {
      tasks.Add(Task.Run(() => {
        for (var j = 0; j < 3; j++) {
          query.Send();
          Thread.Sleep(50);
        }
      }));
    }

    await Task.WhenAll(tasks);
    Assert.True(executionCount < 9);
  }

}