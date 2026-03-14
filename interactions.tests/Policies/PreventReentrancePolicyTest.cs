using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Core.Queries;
using Interactions.Operations;
using Interactions.Policies;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(PreventReentrancePolicy<,>))]
public class PreventReentrancePolicyTest {

  [Fact]
  public void SequentialExecute() {
    IQuery<Unit, Unit> query = Policy.PreventReentrance<Unit>().Apply(Executable.Identity()).AsQuery();

    query.Send();
    query.Send();
  }

  [Fact]
  public void NestedExecution() {
    var query = new Query<Unit, Unit>();
    IQuery<Unit, Unit> inner = Policy.PreventReentrance<Unit>().Apply(query).AsQuery();
    query.Handle(Executable.Create(void () => inner.Send()).AsHandler());
    Assert.Throws<ReentranceException>(() => inner.Send());
  }

  [Fact]
  public void ParallelSequentialExecute() {
    IQuery<Unit, Unit> query = Policy.PreventReentrance<Unit>().Apply(Executable.Identity()).AsQuery();

    Parallel.For(0, 10, _ => {
      query.Send();
      query.Send();
    });
  }

  [Fact]
  public void ParallelNestedExecution() {
    var query = new Query<Unit, Unit>();
    IQuery<Unit, Unit> inner = Policy.PreventReentrance<Unit>().Apply(query).AsQuery();
    query.Handle(Executable.Create(void () => inner.Send()).AsHandler());

    Parallel.For(0, 10, _ => Assert.Throws<ReentranceException>(() => inner.Send()));
  }

}