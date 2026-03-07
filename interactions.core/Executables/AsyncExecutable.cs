namespace Interactions.Core.Executables;

public interface IAsyncExecutable<in TIn, TOut> {

  ValueTask<TOut> Execute(TIn input, CancellationToken token = default);

}