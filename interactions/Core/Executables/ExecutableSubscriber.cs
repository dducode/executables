using Interactions.Subscribers;

namespace Interactions.Core.Executables;

internal sealed class ExecutableSubscriber<T>(IExecutable<T, Unit> inner) : ISubscriber<T>, IExecutor<T, Unit> {

  private readonly IExecutor<T, Unit> _inner = inner.GetExecutor();

  public void Receive(T input) {
    _inner.Execute(input);
  }

  IExecutor<T, Unit> IExecutable<T, Unit>.GetExecutor() {
    return this;
  }

  Unit IExecutor<T, Unit>.Execute(T input) {
    Receive(input);
    return default;
  }

}