namespace Interactions.Core;

public interface IAsyncExecutable<in TIn, TOut> {

  ValueTask<TOut> Execute(TIn input, CancellationToken token = default);

}

public interface IAsyncExecutable<in TIn> {

  ValueTask Execute(TIn input, CancellationToken token = default);

}