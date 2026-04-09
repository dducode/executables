using Executables.Subscribers;

namespace Executables.Core.Subscribers;

internal sealed class AnonymousSubscriber<T>(Action<T> action) : ISubscriber<T>, IExecutor<T, Unit> {

  public void Receive(T input) {
    action(input);
  }

  IExecutor<T, Unit> IExecutable<T, Unit>.GetExecutor() {
    return this;
  }

  Unit IExecutor<T, Unit>.Execute(T arg) {
    Receive(arg);
    return default;
  }

}

internal sealed class AnonymousSubscriber(Action action) : ISubscriber<Unit>, IExecutor<Unit, Unit> {

  public void Receive(Unit input) {
    action();
  }

  IExecutor<Unit, Unit> IExecutable<Unit, Unit>.GetExecutor() {
    return this;
  }

  Unit IExecutor<Unit, Unit>.Execute(Unit arg) {
    Receive(arg);
    return default;
  }

}