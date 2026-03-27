using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Core.Queries;
using Interactions.Executables;
using Interactions.Policies;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(PreventReentrancePolicy<,>))]
public class PreventReentrancePolicyTest {

  [Fact]
  public void SequentialExecute() {
    IQuery<Unit, Unit> query = Executable.Identity().WithPolicy(builder => builder.PreventReentrance()).AsQuery();

    query.Send();
    query.Send();
  }

  [Fact]
  public void NestedExecution() {
    var query = new Query<Unit, Unit>();
    IQuery<Unit, Unit> inner = query.WithPolicy(builder => builder.PreventReentrance()).AsQuery();
    query.Handle(Executable.Create(void () => inner.Send()).AsHandler());
    Assert.Throws<ReentranceException>(() => inner.Send());
  }

  [Fact]
  public void ParallelSequentialExecute() {
    IQuery<Unit, Unit> query = Executable.Identity().WithPolicy(builder => builder.PreventReentrance()).AsQuery();

    Parallel.For(0, 10, _ => {
      query.Send();
      query.Send();
    });
  }

  [Fact]
  public void ParallelNestedExecution() {
    var query = new Query<Unit, Unit>();
    IQuery<Unit, Unit> inner = query.WithPolicy(builder => builder.PreventReentrance()).AsQuery();
    query.Handle(Executable.Create(void () => inner.Send()).AsHandler());

    Parallel.For(0, 10, _ => Assert.Throws<ReentranceException>(() => inner.Send()));
  }

}