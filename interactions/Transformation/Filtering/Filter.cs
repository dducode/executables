namespace Interactions.Transformation.Filtering;

public abstract class Filter<T> : Transformer<IEnumerable<T>, IEnumerable<T>> {

  public override IEnumerable<T> Transform(IEnumerable<T> input) {
    return Apply(input);
  }

  protected abstract IEnumerable<T> Apply(IEnumerable<T> input);

}