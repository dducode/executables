namespace Interactions.Core.Handlers;

internal sealed class AnonymousHandler<T1, T2>(Func<T1, T2> func) : Handler<T1, T2> {

  public override T2 Handle(T1 input) {
    ThrowIfDisposed(nameof(AnonymousHandler<T1, T2>));
    return func(input);
  }

}