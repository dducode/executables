namespace Interactions.Decorators;

internal sealed class AnonymousDecorator<T1, T2>(Func<T1, T2> decoration) : Decorator<T1, T2> {

  public override T2 Decorate(T1 item) {
    return decoration(item);
  }

}