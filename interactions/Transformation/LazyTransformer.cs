using Interactions.Core.Resolvers;

namespace Interactions.Transformation;

internal sealed class LazyTransformer<T1, T2>(IResolver<Transformer<T1, T2>> resolver) : Transformer<T1, T2> {

  private readonly Core.Internal.Lazy<Transformer<T1, T2>> _transformer = new(resolver);

  public override T2 Transform(T1 input) {
    return _transformer.Value.Transform(input);
  }

}