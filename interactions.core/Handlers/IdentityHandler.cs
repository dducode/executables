namespace Interactions.Core.Handlers;

internal sealed class IdentityHandler<T> : Handler<T, T> {

  protected override T ExecuteCore(T input) {
    return input;
  }

}