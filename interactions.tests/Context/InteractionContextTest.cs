using Interactions.Context;
using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Queries;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Interactions.Tests.Context;

[TestSubject(typeof(InteractionContext))]
public class InteractionContextTest(ITestOutputHelper output) {

  [Fact]
  public void SimpleCall() {
    IQuery<Unit, Unit> query = Executable.Create(() => Assert.True(InteractionContext.Current.ContainsKey<string>())).AsQuery();
    Assert.Null(InteractionContext.Current);
    query.Send(context => context.Set("test"));
    Assert.Null(InteractionContext.Current);
  }

  [Fact]
  public void ThrowFromInitCallback() {
    IQuery<Unit, Unit> query = Executable.Create(() => Assert.Fail()).AsQuery();
    Assert.Null(InteractionContext.Current);
    Assert.Throws<InvalidOperationException>(() => query.Send(_ => throw new InvalidOperationException()));
    Assert.Null(InteractionContext.Current);
  }

  [Fact]
  public void ThrowFromQuery() {
    IQuery<Unit, Unit> query = Executable.Create(() => throw new InvalidOperationException()).AsQuery();
    Assert.Null(InteractionContext.Current);
    Assert.Throws<InvalidOperationException>(() => query.Send(context => context.Set(string.Empty)));
    Assert.Null(InteractionContext.Current);
  }

  [Fact]
  public async Task AsyncCall() {
    IAsyncQuery<Unit, Unit> query = AsyncExecutable.Create(async _ => {
      await Task.Yield();
      Assert.True(InteractionContext.Current.ContainsKey<string>());
    }).AsQuery();

    ValueTask task = query.Send(context => context.Set("test"));
    Assert.Null(InteractionContext.Current);
    await task;
    Assert.Null(InteractionContext.Current);
  }

  [Fact]
  public void NestedCall() {
    IQuery<Unit, Unit> inner = Executable.Create(() => Assert.True(InteractionContext.Current.ContainsKey("nested"))).AsQuery();
    IQuery<Unit, Unit> query = Executable.Create(() => {
      Assert.True(InteractionContext.Current.ContainsKey("test"));
      inner.Send(context => context.Set("nested", string.Empty));
      Assert.False(InteractionContext.Current.ContainsKey("nested"));
    }).AsQuery();

    Assert.Null(InteractionContext.Current);
    query.Send(context => context.Set("test", string.Empty));
    Assert.Null(InteractionContext.Current);
  }

  [Fact]
  public async Task NestedAsyncCall() {
    IAsyncQuery<Unit, Unit> inner = AsyncExecutable.Create(async _ => {
      await Task.Yield();
      Assert.True(InteractionContext.Current.ContainsKey("nested"));
    }).AsQuery();

    IAsyncQuery<Unit, Unit> query = AsyncExecutable.Create(async token => {
      Assert.True(InteractionContext.Current.ContainsKey("test"));
      await inner.Send(context => context.Set("nested", string.Empty), token);
      Assert.False(InteractionContext.Current.ContainsKey("nested"));
    }).AsQuery();

    ValueTask task = query.Send(context => context.Set("test", string.Empty));
    Assert.Null(InteractionContext.Current);
    await task;
    Assert.Null(InteractionContext.Current);
  }

  [Fact]
  public async Task NestedAsyncParallelCalls() {
    IAsyncQuery<Unit, Unit> firstInner = AsyncExecutable.Create(async _ => {
      await Task.Yield();
      Assert.True(InteractionContext.Current.ContainsKey("firstNested"));
      Assert.False(InteractionContext.Current.ContainsKey("secondNested"));
    }).AsQuery();

    IAsyncQuery<Unit, Unit> secondInner = AsyncExecutable.Create(async _ => {
      await Task.Yield();
      Assert.True(InteractionContext.Current.ContainsKey("secondNested"));
      Assert.False(InteractionContext.Current.ContainsKey("firstNested"));
    }).AsQuery();

    IAsyncQuery<Unit, Unit> query = AsyncExecutable.Create(async token => {
      Assert.True(InteractionContext.Current.ContainsKey("test"));
      ValueTask t1 = firstInner.Send(context => context.Set("firstNested", string.Empty), token);
      ValueTask t2 = secondInner.Send(context => context.Set("secondNested", string.Empty), token);
      await Task.WhenAll(t1.AsTask(), t2.AsTask());
      Assert.False(InteractionContext.Current.ContainsKey("firstNested"));
      Assert.False(InteractionContext.Current.ContainsKey("secondNested"));
    }).AsQuery();

    ValueTask task = query.Send(context => context.Set("test", string.Empty));
    Assert.Null(InteractionContext.Current);
    await task;
    Assert.Null(InteractionContext.Current);
  }

  [Fact]
  public void PrintHierarchy() {
    IQuery<Unit, Unit> deepInner = Executable.Create(() => output.WriteLine($"{InteractionContext.Current:v}")).AsQuery();
    IQuery<Unit, Unit> inner = Executable.Create(() => deepInner.Send(context => context.Name = nameof(deepInner))).AsQuery();
    IQuery<Unit, Unit> query = Executable.Create(() => inner.Send(context => context.Name = nameof(inner))).AsQuery();

    query.Send(context => context.Name = nameof(query));
    output.WriteLine(string.Empty);
    query.Send(context => context.Name = nameof(query));
  }

}