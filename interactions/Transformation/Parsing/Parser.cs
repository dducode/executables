namespace Interactions.Transformation.Parsing;

public abstract class Parser<T> : Transformer<string, T> {

  public override T Transform(string input) {
    return Parse(input);
  }

  protected abstract T Parse(string input);

}