using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Policies;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(PreventReentrancePolicy<,>))]
public class PreventReentrancePolicyTest {

  [Fact]
  public void SequentialExecute() {
    IExecutable<Unit, Unit> executable = Policy.Of<Unit>().PreventReentrance().Apply(Executable.Identity());

    executable.Execute(default);
    executable.Execute(default);
  }

  [Fact]
  public void NestedExecution() {
    var query = new Query<Unit, Unit>();
    IExecutable<Unit, Unit> executable = Policy.Of<Unit>().PreventReentrance().Apply(query);
    query.Handle(Handler.Create(void () => executable.Execute()));
    Assert.Throws<ReentranceException>(() => executable.Execute(default));
  }

  [Fact]
  public void ParallelSequentialExecute() {
    IExecutable<Unit, Unit> executable = Policy.Of<Unit>().PreventReentrance().Apply(Executable.Identity());

    Parallel.For(0, 10, _ => {
      executable.Execute(default);
      executable.Execute(default);
    });
  }

  [Fact]
  public void ParallelNestedExecution() {
    var query = new Query<Unit, Unit>();
    IExecutable<Unit, Unit> executable = Policy.Of<Unit>().PreventReentrance().Apply(query);
    query.Handle(Handler.Create(void () => executable.Execute()));

    Parallel.For(0, 10, _ => Assert.Throws<ReentranceException>(() => executable.Execute(default)));
  }

}