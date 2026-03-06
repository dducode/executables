namespace Interactions.Core.Handlers;

internal sealed class AnonymousHandler<T1, T2>(Func<T1, T2> func) : Handler<T1, T2> {

  protected override T2 ExecuteCore(T1 input) {
    return func(input);
  }

}