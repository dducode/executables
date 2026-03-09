using Interactions.Core;

namespace Interactions;

public class AsyncMap<T1, T2, T3, T4>(Transformer<T1, T2> inputTransformer, Transformer<T3, T4> outputTransformer)
  : AsyncExecutionOperator<T1, T2, T3, T4> {

  internal static AsyncMap<T1, T1, T2, T2> Identity { get; } = new(Transformer.Identity<T1>(), Transformer.Identity<T2>());

  public override async ValueTask<T4> Invoke(T1 input, IAsyncExecutable<T2, T3> next, CancellationToken token = default) {
    return outputTransformer.Transform(await next.Execute(inputTransformer.Transform(input), token));
  }

}