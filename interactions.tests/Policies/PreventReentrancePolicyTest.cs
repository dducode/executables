using Interactions.Core;
using Interactions.Policies;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(PreventReentrancePolicy<,>))]
public class PreventReentrancePolicyTest {

  [Fact]
  public void SequentialExecute() {
    Policy<Unit, Unit> policy = Policy<Unit, Unit>.PreventReentrance();

    policy.Execute(default, Executable.Identity());
    policy.Execute(default, Executable.Identity());
  }

  [Fact]
  public void NestedExecution() {
    Policy<Unit, Unit> policy = Policy<Unit, Unit>.PreventReentrance();

    policy.Execute(default, Executable.Create((Unit _) => {
      Assert.Throws<ReentranceException>(() => policy.Execute(default, Executable.Identity()));
    }));
  }

  [Fact]
  public void ParallelSequentialExecute() {
    Policy<Unit, Unit> policy = Policy<Unit, Unit>.PreventReentrance();

    Parallel.For(0, 10, _ => {
      policy.Execute(default, Executable.Identity());
      policy.Execute(default, Executable.Identity());
    });
  }

  [Fact]
  public void ParallelNestedExecution() {
    Policy<Unit, Unit> policy = Policy<Unit, Unit>.PreventReentrance();

    Parallel.For(0, 10, _ => policy.Execute(default, Executable.Create((Unit _) => {
      Assert.Throws<ReentranceException>(() => policy.Execute(default, Executable.Identity()));
    })));
  }

}