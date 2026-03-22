using Interactions.Context;
using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Core.Queries;
using Interactions.Executables;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Interactions.Tests.Context;

[TestSubject(typeof(InteractionContext))]
public class InteractionContextTest(ITestOutputHelper output) {

  [Fact]
  public void SimpleCall() {
    IQuery<Unit, Unit> query = Executable
      .Create(() => Assert.True(InteractionContext.Current.ContainsKey<string>()))
      .WithContext(context => context.Set("test"))
      .AsQuery();

    Assert.Null(InteractionContext.Current);
    query.Send();
    Assert.Null(InteractionContext.Current);
  }

  [Fact]
  public void ThrowFromInitCallback() {
    IQuery<Unit, Unit> query = Executable
      .Create(() => Assert.Fail())
      .WithContext(_ => throw new InvalidOperationException())
      .AsQuery();

    Assert.Null(InteractionContext.Current);
    Assert.Throws<InvalidOperationException>(() => query.Send());
    Assert.Null(InteractionContext.Current);
  }

  [Fact]
  public void ThrowFromQuery() {
    IQuery<Unit, Unit> query = Executable
      .Create(() => throw new InvalidOperationException())
      .WithContext(context => context.Set(string.Empty))
      .AsQuery();

    Assert.Null(InteractionContext.Current);
    Assert.Throws<InvalidOperationException>(() => query.Send());
    Assert.Null(InteractionContext.Current);
  }

  [Fact]
  public async Task AsyncCall() {
    IAsyncQuery<Unit, Unit> query = AsyncExecutable
      .Create(async _ => {
        await Task.Yield();
        Assert.True(InteractionContext.Current.ContainsKey<string>());
      })
      .WithContext(context => context.Set("test"))
      .AsQuery();

    ValueTask task = query.Send();
    Assert.Null(InteractionContext.Current);
    await task;
    Assert.Null(InteractionContext.Current);
  }

  [Fact]
  public void NestedCall() {
    IQuery<Unit, Unit> inner = Executable
      .Create(() => Assert.True(InteractionContext.Current.ContainsKey("nested")))
      .WithContext(context => context.Set("nested", string.Empty))
      .AsQuery();

    IQuery<Unit, Unit> query = Executable
      .Create(() => {
        Assert.True(InteractionContext.Current.ContainsKey("test"));
        inner.Send();
        Assert.False(InteractionContext.Current.ContainsKey("nested"));
      })
      .WithContext(context => context.Set("test", string.Empty))
      .AsQuery();

    Assert.Null(InteractionContext.Current);
    query.Send();
    Assert.Null(InteractionContext.Current);
  }

  [Fact]
  public async Task NestedAsyncCall() {
    IAsyncQuery<Unit, Unit> inner = AsyncExecutable
      .Create(async _ => {
        await Task.Yield();
        Assert.True(InteractionContext.Current.ContainsKey("nested"));
      })
      .WithContext(context => context.Set("nested", string.Empty))
      .AsQuery();

    IAsyncQuery<Unit, Unit> query = AsyncExecutable
      .Create(async token => {
        Assert.True(InteractionContext.Current.ContainsKey("test"));
        await inner.Send(token);
        Assert.False(InteractionContext.Current.ContainsKey("nested"));
      })
      .WithContext(context => context.Set("test", string.Empty))
      .AsQuery();

    ValueTask task = query.Send();
    Assert.Null(InteractionContext.Current);
    await task;
    Assert.Null(InteractionContext.Current);
  }

  [Fact]
  public async Task NestedAsyncParallelCalls() {
    IAsyncQuery<Unit, Unit> firstInner = AsyncExecutable
      .Create(async _ => {
        await Task.Yield();
        Assert.True(InteractionContext.Current.ContainsKey("firstNested"));
        Assert.False(InteractionContext.Current.ContainsKey("secondNested"));
      })
      .WithContext(context => context.Set("firstNested", string.Empty))
      .AsQuery();

    IAsyncQuery<Unit, Unit> secondInner = AsyncExecutable
      .Create(async _ => {
        await Task.Yield();
        Assert.True(InteractionContext.Current.ContainsKey("secondNested"));
        Assert.False(InteractionContext.Current.ContainsKey("firstNested"));
      })
      .WithContext(context => context.Set("secondNested", string.Empty))
      .AsQuery();

    IAsyncQuery<Unit, Unit> query = AsyncExecutable
      .Create(async token => {
        Assert.True(InteractionContext.Current.ContainsKey("test"));
        ValueTask t1 = firstInner.Send(token);
        ValueTask t2 = secondInner.Send(token);
        await Task.WhenAll(t1.AsTask(), t2.AsTask());
        Assert.False(InteractionContext.Current.ContainsKey("firstNested"));
        Assert.False(InteractionContext.Current.ContainsKey("secondNested"));
      })
      .WithContext(context => context.Set("test", string.Empty))
      .AsQuery();

    ValueTask task = query.Send();
    Assert.Null(InteractionContext.Current);
    await task;
    Assert.Null(InteractionContext.Current);
  }

  [Fact]
  public void PrintHierarchy() {
    IQuery<Unit, Unit> deepInner = Executable
      .Create(() => output.WriteLine($"{InteractionContext.Current:v}"))
      .WithContext(context => context.Name = nameof(deepInner))
      .AsQuery();

    IQuery<Unit, Unit> inner = Executable
      .Create(() => deepInner.Send())
      .WithContext(context => context.Name = nameof(inner))
      .AsQuery();

    IQuery<Unit, Unit> query = Executable
      .Create(() => inner.Send())
      .WithContext(context => context.Name = nameof(query))
      .AsQuery();

    query.Send();
    output.WriteLine(string.Empty);
    query.Send();
  }

}