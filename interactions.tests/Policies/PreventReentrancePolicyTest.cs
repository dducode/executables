using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Operations;
using Interactions.Policies;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(PreventReentrancePolicy<,>))]
public class PreventReentrancePolicyTest {

  [Fact]
  public void SequentialExecute() {
    IExecutable<Unit> executable = Policy.PreventReentrance<Unit>().Apply(Executable.Identity());

    executable.Execute(default);
    executable.Execute(default);
  }

  [Fact]
  public void NestedExecution() {
    var query = new Query<Unit, Unit>();
    IExecutable<Unit> executable = Policy.PreventReentrance<Unit>().Apply(query);
    query.Handle(Executable.Create(void () => executable.Execute()).AsHandler());
    Assert.Throws<ReentranceException>(() => executable.Execute(default));
  }

  [Fact]
  public void ParallelSequentialExecute() {
    IExecutable<Unit> executable = Policy.PreventReentrance<Unit>().Apply(Executable.Identity());

    Parallel.For(0, 10, _ => {
      executable.Execute(default);
      executable.Execute(default);
    });
  }

  [Fact]
  public void ParallelNestedExecution() {
    var query = new Query<Unit, Unit>();
    IExecutable<Unit> executable = Policy.PreventReentrance<Unit>().Apply(query);
    query.Handle(Executable.Create(void () => executable.Execute()).AsHandler());

    Parallel.For(0, 10, _ => Assert.Throws<ReentranceException>(() => executable.Execute(default)));
  }

}