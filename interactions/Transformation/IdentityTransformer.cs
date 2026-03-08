namespace Interactions.Transformation;

internal sealed class IdentityTransformer<T> : Transformer<T, T> {

  internal static IdentityTransformer<T> Instance { get; } = new();

  private IdentityTransformer() { }

  public override T Transform(T input) {
    return input;
  }

}