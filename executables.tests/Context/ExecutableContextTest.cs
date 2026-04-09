using Executables.Context;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Executables.Tests.Context;

[TestSubject(typeof(ExecutableContext))]
public class ExecutableContextTest(ITestOutputHelper output) {

  [Fact]
  public void SimpleCall() {
    IExecutor<Unit, Unit> executor = Executable
      .Create(() => Assert.True(ExecutableContext.Current.ContainsKey<string>()))
      .GetExecutor()
      .WithContext(context => context.Set("test"));

    Assert.Null(ExecutableContext.Current);
    executor.Execute();
    Assert.Null(ExecutableContext.Current);
  }

  [Fact]
  public void ThrowFromInitCallback() {
    var executor = Executable
      .Create(() => Assert.Fail())
      .GetExecutor()
      .WithContext(_ => throw new InvalidOperationException());

    Assert.Null(ExecutableContext.Current);
    Assert.Throws<InvalidOperationException>(() => executor.Execute());
    Assert.Null(ExecutableContext.Current);
  }

  [Fact]
  public void ThrowFromQuery() {
    IExecutor<Unit, Unit> executor = Executable
      .Create(() => throw new InvalidOperationException())
      .GetExecutor()
      .WithContext(context => context.Set(string.Empty));

    Assert.Null(ExecutableContext.Current);
    Assert.Throws<InvalidOperationException>(() => executor.Execute());
    Assert.Null(ExecutableContext.Current);
  }

  [Fact]
  public async Task AsyncCall() {
    IAsyncExecutor<Unit, Unit> executor = AsyncExecutable
      .Create(async _ => {
        await Task.Yield();
        Assert.True(ExecutableContext.Current.ContainsKey<string>());
      })
      .GetExecutor()
      .WithContext(context => context.Set("test"));

    ValueTask task = executor.Execute();
    Assert.Null(ExecutableContext.Current);
    await task;
    Assert.Null(ExecutableContext.Current);
  }

  [Fact]
  public void NestedCall() {
    IExecutor<Unit, Unit> inner = Executable
      .Create(() => Assert.True(ExecutableContext.Current.ContainsKey("nested")))
      .GetExecutor()
      .WithContext(context => context.Set("nested", string.Empty));

    IExecutor<Unit, Unit> executor = Executable
      .Create(() => {
        Assert.True(ExecutableContext.Current.ContainsKey("test"));
        inner.Execute();
        Assert.False(ExecutableContext.Current.ContainsKey("nested"));
      })
      .GetExecutor()
      .WithContext(context => context.Set("test", string.Empty));

    Assert.Null(ExecutableContext.Current);
    executor.Execute();
    Assert.Null(ExecutableContext.Current);
  }

  [Fact]
  public async Task NestedAsyncCall() {
    IAsyncExecutor<Unit, Unit> inner = AsyncExecutable
      .Create(async _ => {
        await Task.Yield();
        Assert.True(ExecutableContext.Current.ContainsKey("nested"));
      })
      .GetExecutor()
      .WithContext(context => context.Set("nested", string.Empty));

    IAsyncExecutor<Unit, Unit> executor = AsyncExecutable
      .Create(async token => {
        Assert.True(ExecutableContext.Current.ContainsKey("test"));
        await inner.Execute(token);
        Assert.False(ExecutableContext.Current.ContainsKey("nested"));
      })
      .GetExecutor()
      .WithContext(context => context.Set("test", string.Empty));

    ValueTask task = executor.Execute();
    Assert.Null(ExecutableContext.Current);
    await task;
    Assert.Null(ExecutableContext.Current);
  }

  [Fact]
  public async Task NestedAsyncParallelCalls() {
    IAsyncExecutor<Unit, Unit> firstInner = AsyncExecutable
      .Create(async _ => {
        await Task.Yield();
        Assert.True(ExecutableContext.Current.ContainsKey("firstNested"));
        Assert.False(ExecutableContext.Current.ContainsKey("secondNested"));
      })
      .GetExecutor()
      .WithContext(context => context.Set("firstNested", string.Empty));

    IAsyncExecutor<Unit, Unit> secondInner = AsyncExecutable
      .Create(async _ => {
        await Task.Yield();
        Assert.True(ExecutableContext.Current.ContainsKey("secondNested"));
        Assert.False(ExecutableContext.Current.ContainsKey("firstNested"));
      })
      .GetExecutor()
      .WithContext(context => context.Set("secondNested", string.Empty));

    IAsyncExecutor<Unit, Unit> executor = AsyncExecutable
      .Create(async token => {
        Assert.True(ExecutableContext.Current.ContainsKey("test"));
        ValueTask t1 = firstInner.Execute(token);
        ValueTask t2 = secondInner.Execute(token);
        await Task.WhenAll(t1.AsTask(), t2.AsTask());
        Assert.False(ExecutableContext.Current.ContainsKey("firstNested"));
        Assert.False(ExecutableContext.Current.ContainsKey("secondNested"));
      })
      .GetExecutor()
      .WithContext(context => context.Set("test", string.Empty));

    ValueTask task = executor.Execute();
    Assert.Null(ExecutableContext.Current);
    await task;
    Assert.Null(ExecutableContext.Current);
  }

  [Fact]
  public void PrintHierarchy() {
    IExecutor<Unit, Unit> deepInner = Executable
      .Create(() => output.WriteLine($"{ExecutableContext.Current:v}"))
      .GetExecutor()
      .WithContext(context => context.Name = nameof(deepInner));

    IExecutor<Unit, Unit> inner = Executable
      .Create(() => deepInner.Execute())
      .GetExecutor()
      .WithContext(context => context.Name = nameof(inner));

    IExecutor<Unit, Unit> query = Executable
      .Create(() => inner.Execute())
      .GetExecutor()
      .WithContext(context => context.Name = nameof(query));

    query.Execute();
    output.WriteLine(string.Empty);
    query.Execute();
  }

}