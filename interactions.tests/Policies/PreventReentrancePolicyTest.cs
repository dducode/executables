using Interactions.Core;
using Interactions.Policies;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(PreventReentrancePolicy<,>))]
public class PreventReentrancePolicyTest {

  [Fact]
  public void SequentialExecute() {
    Policy<Unit, Unit> policy = Policy<Unit, Unit>.PreventReentrance();

    policy.Execute(default, _ => default);
    policy.Execute(default, _ => default);
  }

  [Fact]
  public void NestedExecution() {
    Policy<Unit, Unit> policy = Policy<Unit, Unit>.PreventReentrance();

    policy.Execute(default, _ => {
      Assert.Throws<ReentranceException>(() => policy.Execute(default, _ => default));
      return default;
    });
  }

  [Fact]
  public void ParallelSequentialExecute() {
    Policy<Unit, Unit> policy = Policy<Unit, Unit>.PreventReentrance();

    Parallel.For(0, 10, _ => {
      policy.Execute(default, _ => default);
      policy.Execute(default, _ => default);
    });
  }

  [Fact]
  public void ParallelNestedExecution() {
    Policy<Unit, Unit> policy = Policy<Unit, Unit>.PreventReentrance();

    Parallel.For(0, 10, _ => policy.Execute(default, _ => {
      Assert.Throws<ReentranceException>(() => policy.Execute(default, _ => default));
      return default;
    }));
  }

}