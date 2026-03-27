using Interactions.Core.Executables;
using Interactions.Queries;
using JetBrains.Annotations;

namespace Interactions.Tests.Executables;

[TestSubject(typeof(RaceExecutable<,>))]
public class RaceExecutableTest {

  [Theory]
  [InlineData(10, 100, 2, 1)]
  [InlineData(100, 10, 11, 1)]
  public async Task SimpleRace(int firstDelay, int secondDelay, int expected, int value) {
    IAsyncQuery<int, int> query = AsyncExecutable
      .Identity<int>()
      .Race(
        async (x, token) => {
          await Task.Delay(firstDelay, token);
          return x * 2;
        },
        async (x, token) => {
          await Task.Delay(secondDelay, token);
          return x + 10;
        })
      .AsQuery();

    Assert.Equal(expected, await query.Send(value));
  }

  [Theory]
  [InlineData(10, 1)]
  [InlineData(50, 5)]
  public async Task ManyRace(int expected, int value) {
    IAsyncQuery<int, int> query = AsyncExecutable
      .Identity<int>()
      .Race(
        async (x, token) => {
          await Task.Delay(10, token);
          return x * 2;
        },
        async (x, token) => {
          await Task.Delay(50, token);
          return x + 10;
        },
        (x, _) => ValueTask.FromResult(x * 10),
        async (x, token) => {
          await Task.Delay(250, token);
          return x + 100;
        },
        async (x, token) => {
          await Task.Delay(100, token);
          return x * 100;
        })
      .AsQuery();

    Assert.Equal(expected, await query.Send(value));
  }

  [Fact]
  public async Task RaceWithLoserCancellation() {
    var canceled = false;

    IAsyncQuery<int, int> query = AsyncExecutable
      .Identity<int>()
      .Race(
        async (x, token) => {
          try {
            await Task.Delay(100, token);
          }
          catch (OperationCanceledException) {
            canceled = true;
          }

          return x + 10;
        },
        async (x, token) => {
          await Task.Delay(10, token);
          return x * 2;
        })
      .WithPolicy(builder => builder.CancelAfterCompletion())
      .AsQuery();

    Assert.Equal(2, await query.Send(1));
    Assert.True(canceled);
  }

  [Fact]
  public async Task ThrowExceptionWhenAnyFaulted() {
    IAsyncQuery<int, int> query = AsyncExecutable
      .Identity<int>()
      .Race(
        async (x, token) => {
          await Task.Delay(10, token);
          return x + 2;
        },
        (_, _) => throw new InvalidOperationException(),
        async (x, token) => {
          await Task.Delay(50, token);
          return x * 2;
        })
      .AsQuery();

    await Assert.ThrowsAsync<InvalidOperationException>(async () => await query.Send(0));
  }

  [Theory]
  [InlineData("meow", 10, 50)]
  [InlineData("woof", 50, 10)]
  public async Task CatEitherDogRace(string expectedSound, int catDelay, int dogDelay) {
    IAsyncQuery<Unit, string> query = AsyncExecutable
      .Identity()
      .Race(
        async ValueTask<Animal> (_, token) => {
          await Task.Delay(catDelay, token);
          return new Cat();
        },
        async ValueTask<Animal> (_, token) => {
          await Task.Delay(dogDelay, token);
          return new Dog();
        })
      .Then(animal => animal.MakeSound())
      .AsQuery();

    Assert.Equal(expectedSound, await query.Send());
  }

}

file abstract class Animal {

  public abstract string MakeSound();

}

file sealed class Cat : Animal {

  public override string MakeSound() {
    return "meow";
  }

}

file sealed class Dog : Animal {

  public override string MakeSound() {
    return "woof";
  }

}