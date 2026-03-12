using Interactions.Context;
using Interactions.Core;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Interactions.Tests.Context;

[TestSubject(typeof(InteractionContext))]
public class InteractionContextTest(ITestOutputHelper output) {

  [Fact]
  public void SimpleCall() {
    IExecutable<Unit> executable = Executable.Create(() => Assert.True(InteractionContext.Current.ContainsKey<string>()));
    Assert.Null(InteractionContext.Current);
    executable.Execute(context => context.Set("test"));
    Assert.Null(InteractionContext.Current);
  }

  [Fact]
  public async Task AsyncCall() {
    IAsyncExecutable<Unit> executable = AsyncExecutable.Create(async _ => {
      await Task.Yield();
      Assert.True(InteractionContext.Current.ContainsKey<string>());
    });

    ValueTask task = executable.Execute(context => context.Set("test"));
    Assert.Null(InteractionContext.Current);
    await task;
    Assert.Null(InteractionContext.Current);
  }

  [Fact]
  public void NestedCall() {
    IExecutable<Unit> inner = Executable.Create(() => Assert.True(InteractionContext.Current.ContainsKey("nested")));
    IExecutable<Unit> executable = Executable.Create(() => {
      Assert.True(InteractionContext.Current.ContainsKey("test"));
      inner.Execute(context => context.Set("nested", string.Empty));
      Assert.False(InteractionContext.Current.ContainsKey("nested"));
    });

    Assert.Null(InteractionContext.Current);
    executable.Execute(context => context.Set("test", string.Empty));
    Assert.Null(InteractionContext.Current);
  }

  [Fact]
  public async Task NestedAsyncCall() {
    IAsyncExecutable<Unit> inner = AsyncExecutable.Create(async _ => {
      await Task.Yield();
      Assert.True(InteractionContext.Current.ContainsKey("nested"));
    });

    IAsyncExecutable<Unit> executable = AsyncExecutable.Create(async token => {
      Assert.True(InteractionContext.Current.ContainsKey("test"));
      await inner.Execute(context => context.Set("nested", string.Empty), token);
      Assert.False(InteractionContext.Current.ContainsKey("nested"));
    });

    ValueTask task = executable.Execute(context => context.Set("test", string.Empty));
    Assert.Null(InteractionContext.Current);
    await task;
    Assert.Null(InteractionContext.Current);
  }

  [Fact]
  public async Task NestedAsyncParallelCalls() {
    IAsyncExecutable<Unit> firstInner = AsyncExecutable.Create(async _ => {
      await Task.Yield();
      Assert.True(InteractionContext.Current.ContainsKey("firstNested"));
      Assert.False(InteractionContext.Current.ContainsKey("secondNested"));
    });

    IAsyncExecutable<Unit> secondInner = AsyncExecutable.Create(async _ => {
      await Task.Yield();
      Assert.True(InteractionContext.Current.ContainsKey("secondNested"));
      Assert.False(InteractionContext.Current.ContainsKey("firstNested"));
    });

    IAsyncExecutable<Unit> executable = AsyncExecutable.Create(async token => {
      Assert.True(InteractionContext.Current.ContainsKey("test"));
      ValueTask t1 = firstInner.Execute(context => context.Set("firstNested", string.Empty), token);
      ValueTask t2 = secondInner.Execute(context => context.Set("secondNested", string.Empty), token);
      await Task.WhenAll(t1.AsTask(), t2.AsTask());
      Assert.False(InteractionContext.Current.ContainsKey("firstNested"));
      Assert.False(InteractionContext.Current.ContainsKey("secondNested"));
    });

    ValueTask task = executable.Execute(context => context.Set("test", string.Empty));
    Assert.Null(InteractionContext.Current);
    await task;
    Assert.Null(InteractionContext.Current);
  }

  [Fact]
  public void PrintHierarchy() {
    IExecutable<Unit> deepInner = Executable.Create(() => output.WriteLine($"{InteractionContext.Current:v}"));
    IExecutable<Unit> inner = Executable.Create(() => deepInner.Execute(default, context => context.Name = nameof(deepInner)));
    IExecutable<Unit> executable = Executable.Create(() => inner.Execute(default, context => context.Name = nameof(inner)));

    executable.Execute(context => context.Name = nameof(executable));
    output.WriteLine("");
    executable.Execute(context => context.Name = nameof(executable));
  }

}