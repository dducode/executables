using Interactions.Core.Providers;

namespace Interactions.Transformation;

internal sealed class DynamicTransformer<T1, T2>(IProvider<Transformer<T1, T2>> provider) : Transformer<T1, T2> {

  public override T2 Transform(T1 input) {
    Transformer<T1, T2> transformer = provider.Get();
    return transformer != null ? transformer.Transform(input) : throw new InvalidOperationException($"Cannot resolve transformer by {provider.GetType().Name}");
  }

}