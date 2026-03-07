using Interactions.Core.Handlers;

namespace Interactions.Handlers;

internal sealed class TransitiveHandler<T>(Action<T> action) : Handler<T, T> {

  protected override T ExecuteCore(T input) {
    action(input);
    return input;
  }

}