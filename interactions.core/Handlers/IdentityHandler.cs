namespace Interactions.Core.Handlers;

internal sealed class IdentityHandler<T> : Handler<T, T> {

  protected override T HandleCore(T input) {
    return input;
  }

}