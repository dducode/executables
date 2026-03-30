using System.Diagnostics.Contracts;
using Executables.Core.Queries;
using Executables.Internal;

namespace Executables.Queries;

public static class AsyncQueriesExtensions {

  /// <summary>
  /// Sends a parameterless asynchronous query.
  /// </summary>
  /// <returns>Asynchronous query result.</returns>
  public static ValueTask<T> Send<T>(this IAsyncQuery<Unit, T> query, CancellationToken token = default) {
    return query.Send(default, token);
  }

  /// <summary>
  /// Sends a parameterless asynchronous query with no result.
  /// </summary>
  public static async ValueTask Send(this IAsyncQuery<Unit, Unit> query, CancellationToken token = default) {
    await query.Send(default, token);
  }

  /// <summary>
  /// Chains two asynchronous queries into a single asynchronous query.
  /// </summary>
  /// <returns>Asynchronous chained query.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncQuery<T1, T3> Connect<T1, T2, T3>(this IAsyncQuery<T1, T2> query, IAsyncQuery<T2, T3> other) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new AsyncChainedQuery<T1, T2, T3>(query, other);
  }

  /// <summary>
  /// Connects an asynchronous query to a synchronous query.
  /// </summary>
  /// <returns>Asynchronous chained query.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncQuery<T1, T3> Connect<T1, T2, T3>(this IAsyncQuery<T1, T2> query, IQuery<T2, T3> other) {
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return query.Connect(other.ToAsyncQuery());
  }

}