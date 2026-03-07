namespace Interactions.Core.Executables;

public interface IExecutable<in TIn, out TOut> {

  TOut Execute(TIn input);

}