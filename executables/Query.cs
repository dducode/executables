using Executables.Handling;

namespace Executables;

/// <summary>
/// Represents a query that returns a result.
/// </summary>
/// <typeparam name="T1">Type of the query input.</typeparam>
/// <typeparam name="T2">Type of the query result.</typeparam>
public interface IQuery<in T1, out T2> : IExecutable<T1, T2> {

  /// <summary>
  /// Sends the query.
  /// </summary>
  /// <param name="input">Query input.</param>
  /// <returns>Query result.</returns>
  T2 Send(T1 input);

}

/// <summary>
/// Default query implementation backed by a registered handler.
/// </summary>
/// <typeparam name="T1">Type of the query input.</typeparam>
/// <typeparam name="T2">Type of the query result.</typeparam>
public sealed class Query<T1, T2> : Handleable<T1, T2>, IQuery<T1, T2> {

  public T2 Send(T1 input) {
    Handler<T1, T2> handler = Handler;
    return handler != null ? handler.Handle(input) : throw new MissingHandlerException("Cannot handle query");
  }

  public Executor GetExecutor() {
    return new Executor(this);
  }

  IExecutor<T1, T2> IExecutable<T1, T2>.GetExecutor() {
    return GetExecutor();
  }

  public readonly struct Executor(IQuery<T1, T2> query) : IExecutor<T1, T2> {

    public T2 Execute(T1 input) {
      return query.Send(input);
    }

  }

}