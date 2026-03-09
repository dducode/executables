using Interactions.Core;
using Interactions.Transformation;

namespace Interactions.Maps;

internal sealed class SymmetricMap<T1, T2>(SymmetricTransformer<T1, T2> transformer) : ExecutionOperator<T1, T2, T2, T1> {

  public override T1 Invoke(T1 input, IExecutable<T2, T2> next) {
    return transformer.InverseTransform(next.Execute(transformer.Transform(input)));
  }

}