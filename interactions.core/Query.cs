namespace Interactions.Core;

public interface IQuery<in T1, out T2> : IExecutable<T1, T2> { }

public class Query<T1, T2> : Handleable<T1, T2>, IQuery<T1, T2> {

  public virtual T2 Execute(T1 input) {
    Handler<T1, T2> handler = Handler;
    return handler != null ? handler.Execute(input) : throw new MissingHandlerException("Cannot handle query");
  }

}