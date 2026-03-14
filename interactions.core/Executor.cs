namespace Interactions.Core;

public interface IExecutor<in TIn, out TOut> {

  TOut Execute(TIn input);

}