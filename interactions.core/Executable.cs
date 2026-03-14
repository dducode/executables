namespace Interactions.Core;

public interface IExecutable<in TIn, out TOut> {

  IExecutor<TIn, TOut> GetExecutor();

}