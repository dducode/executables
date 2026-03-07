namespace Interactions.Decorators;

public abstract class Decorator<T1, T2> {

  public abstract T2 Decorate(T1 item);

}