namespace Interactions.Core;

public interface IExecutable<in TIn, out TOut> {

  TOut Execute(TIn input);

}

public interface IExecutable<in TIn> {

  void Execute(TIn input);

}