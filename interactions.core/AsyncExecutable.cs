namespace Interactions.Core;

public interface IAsyncExecutable<in TIn, TOut> {

  ValueTask<TOut> Execute(TIn input, CancellationToken token = default);

}