namespace Interactions.Core.Handlers;

internal sealed class IdentityHandler<T> : Handler<T, T> {

  public override T Handle(T input) {
    ThrowIfDisposed(nameof(IdentityHandler<T>));
    return input;
  }

}