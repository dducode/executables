namespace Interactions.Core;

public interface IAsyncExecutable<in TIn, TOut> {

  IAsyncExecutor<TIn, TOut> GetExecutor();

}