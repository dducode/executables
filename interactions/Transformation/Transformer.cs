namespace Interactions.Transformation;

public abstract class Transformer<T1, T2> {

  public abstract T2 Transform(T1 input);

}