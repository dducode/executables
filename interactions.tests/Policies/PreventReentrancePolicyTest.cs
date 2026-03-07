using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Policies;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(PreventReentrancePolicy<,>))]
public class PreventReentrancePolicyTest {

  [Fact]
  public void SequentialExecute() {
    Policy<Unit, Unit> policy = Policy<Unit, Unit>.PreventReentrance();

    policy.Invoke(default, Executable.Identity());
    policy.Invoke(default, Executable.Identity());
  }

  [Fact]
  public void NestedExecution() {
    Policy<Unit, Unit> policy = Policy<Unit, Unit>.PreventReentrance();

    policy.Invoke(default, Executable.Create((Unit _) => {
      Assert.Throws<ReentranceException>(() => policy.Invoke(default, Executable.Identity()));
    }));
  }

  [Fact]
  public void ParallelSequentialExecute() {
    Policy<Unit, Unit> policy = Policy<Unit, Unit>.PreventReentrance();

    Parallel.For(0, 10, _ => {
      policy.Invoke(default, Executable.Identity());
      policy.Invoke(default, Executable.Identity());
    });
  }

  [Fact]
  public void ParallelNestedExecution() {
    Policy<Unit, Unit> policy = Policy<Unit, Unit>.PreventReentrance();

    Parallel.For(0, 10, _ => policy.Invoke(default, Executable.Create((Unit _) => {
      Assert.Throws<ReentranceException>(() => policy.Invoke(default, Executable.Identity()));
    })));
  }

}