namespace Interactions.Decorators;

internal sealed class CompositeDecorator<T1, T2, T3>(Decorator<T1, T2> first, Decorator<T2, T3> second) : Decorator<T1, T3> {

  public override T3 Decorate(T1 item) {
    return second.Decorate(first.Decorate(item));
  }

}