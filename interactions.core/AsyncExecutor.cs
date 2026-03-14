namespace Interactions.Core;

public interface IAsyncExecutor<in TIn, TOut> {

  ValueTask<TOut> Execute(TIn input, CancellationToken token = default);

}