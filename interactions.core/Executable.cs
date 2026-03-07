namespace Interactions.Core;

public interface IExecutable<in TIn, out TOut> {

  TOut Execute(TIn input);

}