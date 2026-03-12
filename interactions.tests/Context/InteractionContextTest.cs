using Interactions.Context;
using Interactions.Core;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Interactions.Tests.Context;

[TestSubject(typeof(InteractionContext))]
public class InteractionContextTest(ITestOutputHelper output) {

  [Fact]
  public void SimpleCallWithContext() {
    IExecutable<Unit> executable = Executable.Create(() => Assert.True(InteractionContext.Current.ContainsKey<string>()));
    executable.Execute(default, context => context.Set("test"));
  }

  [Fact]
  public void NestedCall() {
    IExecutable<Unit> inner = Executable.Create(() => Assert.True(InteractionContext.Current.ContainsKey("nested")));
    IExecutable<Unit> executable = Executable.Create(() => {
      Assert.True(InteractionContext.Current.ContainsKey("test"));
      inner.Execute(default, context => context.Set("nested", string.Empty));
      Assert.False(InteractionContext.Current.ContainsKey("nested"));
    });

    executable.Execute(default, context => context.Set("test", string.Empty));
  }

  [Fact]
  public void PrintHierarchy() {
    IExecutable<Unit> deepInner = Executable.Create(() => output.WriteLine($"{InteractionContext.Current:v}"));
    IExecutable<Unit> inner = Executable.Create(() => deepInner.Execute(default, context => context.Name = nameof(deepInner)));
    IExecutable<Unit> executable = Executable.Create(() => inner.Execute(default, context => context.Name = nameof(inner)));

    executable.Execute(default, context => context.Name = nameof(executable));
    output.WriteLine("");
    executable.Execute(default, context => context.Name = nameof(executable));
  }

}