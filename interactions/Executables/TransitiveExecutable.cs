using Interactions.Core;

namespace Interactions.Executables;

internal sealed class TransitiveExecutable<T>(Action<T> action) : IExecutable<T, T> {

  public T Execute(T input) {
    action(input);
    return input;
  }

}