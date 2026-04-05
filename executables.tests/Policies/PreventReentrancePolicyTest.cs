using Executables.Core.Policies;
using Executables.Policies;
using JetBrains.Annotations;

namespace Executables.Tests.Policies;

[TestSubject(typeof(PreventReentrancePolicy<,>))]
public class PreventReentrancePolicyTest {

  [Fact]
  public void SequentialExecute() {
    IExecutor<Unit, Unit> executor = Executable.Identity().GetExecutor().WithPolicy(builder => builder.PreventReentrance());

    executor.Execute();
    executor.Execute();
  }

  [Fact]
  public void NestedExecution() {
    var query = new Query<Unit, Unit>();
    IExecutor<Unit, Unit> executor = query.GetExecutor().WithPolicy(builder => builder.PreventReentrance());
    query.Handle(Executable.Create(void () => executor.Execute()).AsHandler());
    Assert.Throws<ReentranceException>(() => executor.Execute());
  }

  [Fact]
  public void ParallelSequentialExecute() {
    IExecutor<Unit, Unit> executor = Executable.Identity().GetExecutor().WithPolicy(builder => builder.PreventReentrance());

    Parallel.For(0, 10, _ => {
      executor.Execute();
      executor.Execute();
    });
  }

  [Fact]
  public void ParallelNestedExecution() {
    var query = new Query<Unit, Unit>();
    IExecutor<Unit, Unit> executor = query.GetExecutor().WithPolicy(builder => builder.PreventReentrance());
    query.Handle(Executable.Create(void () => executor.Execute()).AsHandler());

    Parallel.For(0, 10, _ => Assert.Throws<ReentranceException>(() => executor.Execute()));
  }

}