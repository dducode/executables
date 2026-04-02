using Executables.Handling;

namespace Executables;

/// <summary>
/// Represents an asynchronous query that returns a result.
/// </summary>
/// <typeparam name="T1">Type of the query input.</typeparam>
/// <typeparam name="T2">Type of the query result.</typeparam>
public interface IAsyncQuery<in T1, T2> : IAsyncExecutable<T1, T2> {

  /// <summary>
  /// Sends the query.
  /// </summary>
  /// <param name="input">Query input.</param>
  /// <param name="token">Cancellation token.</param>
  /// <returns>Asynchronous query result.</returns>
  ValueTask<T2> Send(T1 input, CancellationToken token = default);

}

/// <summary>
/// Default asynchronous query implementation backed by a registered handler.
/// </summary>
/// <typeparam name="T1">Type of the query input.</typeparam>
/// <typeparam name="T2">Type of the query result.</typeparam>
public sealed class AsyncQuery<T1, T2> : AsyncHandleable<T1, T2>, IAsyncQuery<T1, T2> {

  public ValueTask<T2> Send(T1 input, CancellationToken token = default) {
    AsyncHandler<T1, T2> handler = Handler;
    if (handler == null)
      throw new MissingHandlerException("Cannot handle query");
    token.ThrowIfCancellationRequested();
    return handler.Handle(input, token);
  }

  public Executor GetExecutor() {
    return new Executor(this);
  }

  IAsyncExecutor<T1, T2> IAsyncExecutable<T1, T2>.GetExecutor() {
    return GetExecutor();
  }

  public readonly struct Executor(IAsyncQuery<T1, T2> query) : IAsyncExecutor<T1, T2>, IEquatable<Executor> {

    private readonly IAsyncQuery<T1, T2> _query = query;

    public ValueTask<T2> Execute(T1 input, CancellationToken token = default) {
      return _query.Send(input, token);
    }

    public bool Equals(Executor other) {
      return _query.Equals(other._query);
    }

    public override bool Equals(object obj) {
      return obj is Executor other && Equals(other);
    }

    public override int GetHashCode() {
      return _query.GetHashCode();
    }

    public static bool operator ==(Executor left, Executor right) {
      return left.Equals(right);
    }

    public static bool operator !=(Executor left, Executor right) {
      return !(left == right);
    }

  }

}