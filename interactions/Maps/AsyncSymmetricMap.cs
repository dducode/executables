using Interactions.Core;
using Interactions.Transformation;

namespace Interactions.Maps;

internal sealed class AsyncSymmetricMap<T1, T2>(SymmetricTransformer<T1, T2> transformer) : AsyncExecutionOperator<T1, T2, T2, T1> {

  public override async ValueTask<T1> Invoke(T1 input, IAsyncExecutable<T2, T2> next, CancellationToken token = default) {
    return transformer.InverseTransform(await next.Execute(transformer.Transform(input), token));
  }

}