namespace Interactions.Core.Executables;

internal sealed class TransitiveExecutable<T>(Action<T> action) : IExecutable<T, T>, IExecutor<T, T> {

  IExecutor<T, T> IExecutable<T, T>.GetExecutor() {
    return this;
  }

  T IExecutor<T, T>.Execute(T input) {
    action(input);
    return input;
  }

}