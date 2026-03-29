using Executables.Context;
using Executables.Queries;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Executables.Tests.Context;

[TestSubject(typeof(ExecutableContext))]
public class ExecutableContextTest(ITestOutputHelper output) {

  [Fact]
  public void SimpleCall() {
    IQuery<Unit, Unit> query = Executable
      .Create(() => Assert.True(ExecutableContext.Current.ContainsKey<string>()))
      .WithContext(context => context.Set("test"))
      .AsQuery();

    Assert.Null(ExecutableContext.Current);
    query.Send();
    Assert.Null(ExecutableContext.Current);
  }

  [Fact]
  public void ThrowFromInitCallback() {
    IQuery<Unit, Unit> query = Executable
      .Create(() => Assert.Fail())
      .WithContext(_ => throw new InvalidOperationException())
      .AsQuery();

    Assert.Null(ExecutableContext.Current);
    Assert.Throws<InvalidOperationException>(() => query.Send());
    Assert.Null(ExecutableContext.Current);
  }

  [Fact]
  public void ThrowFromQuery() {
    IQuery<Unit, Unit> query = Executable
      .Create(() => throw new InvalidOperationException())
      .WithContext(context => context.Set(string.Empty))
      .AsQuery();

    Assert.Null(ExecutableContext.Current);
    Assert.Throws<InvalidOperationException>(() => query.Send());
    Assert.Null(ExecutableContext.Current);
  }

  [Fact]
  public async Task AsyncCall() {
    IAsyncQuery<Unit, Unit> query = AsyncExecutable
      .Create(async _ => {
        await Task.Yield();
        Assert.True(ExecutableContext.Current.ContainsKey<string>());
      })
      .WithContext(context => context.Set("test"))
      .AsQuery();

    ValueTask task = query.Send();
    Assert.Null(ExecutableContext.Current);
    await task;
    Assert.Null(ExecutableContext.Current);
  }

  [Fact]
  public void NestedCall() {
    IQuery<Unit, Unit> inner = Executable
      .Create(() => Assert.True(ExecutableContext.Current.ContainsKey("nested")))
      .WithContext(context => context.Set("nested", string.Empty))
      .AsQuery();

    IQuery<Unit, Unit> query = Executable
      .Create(() => {
        Assert.True(ExecutableContext.Current.ContainsKey("test"));
        inner.Send();
        Assert.False(ExecutableContext.Current.ContainsKey("nested"));
      })
      .WithContext(context => context.Set("test", string.Empty))
      .AsQuery();

    Assert.Null(ExecutableContext.Current);
    query.Send();
    Assert.Null(ExecutableContext.Current);
  }

  [Fact]
  public async Task NestedAsyncCall() {
    IAsyncQuery<Unit, Unit> inner = AsyncExecutable
      .Create(async _ => {
        await Task.Yield();
        Assert.True(ExecutableContext.Current.ContainsKey("nested"));
      })
      .WithContext(context => context.Set("nested", string.Empty))
      .AsQuery();

    IAsyncQuery<Unit, Unit> query = AsyncExecutable
      .Create(async token => {
        Assert.True(ExecutableContext.Current.ContainsKey("test"));
        await inner.Send(token);
        Assert.False(ExecutableContext.Current.ContainsKey("nested"));
      })
      .WithContext(context => context.Set("test", string.Empty))
      .AsQuery();

    ValueTask task = query.Send();
    Assert.Null(ExecutableContext.Current);
    await task;
    Assert.Null(ExecutableContext.Current);
  }

  [Fact]
  public async Task NestedAsyncParallelCalls() {
    IAsyncQuery<Unit, Unit> firstInner = AsyncExecutable
      .Create(async _ => {
        await Task.Yield();
        Assert.True(ExecutableContext.Current.ContainsKey("firstNested"));
        Assert.False(ExecutableContext.Current.ContainsKey("secondNested"));
      })
      .WithContext(context => context.Set("firstNested", string.Empty))
      .AsQuery();

    IAsyncQuery<Unit, Unit> secondInner = AsyncExecutable
      .Create(async _ => {
        await Task.Yield();
        Assert.True(ExecutableContext.Current.ContainsKey("secondNested"));
        Assert.False(ExecutableContext.Current.ContainsKey("firstNested"));
      })
      .WithContext(context => context.Set("secondNested", string.Empty))
      .AsQuery();

    IAsyncQuery<Unit, Unit> query = AsyncExecutable
      .Create(async token => {
        Assert.True(ExecutableContext.Current.ContainsKey("test"));
        ValueTask t1 = firstInner.Send(token);
        ValueTask t2 = secondInner.Send(token);
        await Task.WhenAll(t1.AsTask(), t2.AsTask());
        Assert.False(ExecutableContext.Current.ContainsKey("firstNested"));
        Assert.False(ExecutableContext.Current.ContainsKey("secondNested"));
      })
      .WithContext(context => context.Set("test", string.Empty))
      .AsQuery();

    ValueTask task = query.Send();
    Assert.Null(ExecutableContext.Current);
    await task;
    Assert.Null(ExecutableContext.Current);
  }

  [Fact]
  public void PrintHierarchy() {
    IQuery<Unit, Unit> deepInner = Executable
      .Create(() => output.WriteLine($"{ExecutableContext.Current:v}"))
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