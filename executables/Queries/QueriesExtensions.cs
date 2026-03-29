using System.Diagnostics.Contracts;
using Executables.Core.Queries;
using Executables.Internal;

namespace Executables.Queries;

public static partial class QueriesExtensions {

  /// <summary>
  /// Sends a parameterless query.
  /// </summary>
  /// <returns>Query result.</returns>
  public static T Send<T>(this IQuery<Unit, T> query) {
    return query.Send(default);
  }

  /// <summary>
  /// Sends a parameterless query with no result.
  /// </summary>
  public static void Send(this IQuery<Unit, Unit> query) {
    query.Send(default);
  }

  /// <summary>
  /// Chains two synchronous queries into a single query.
  /// </summary>
  /// <returns>Chained query that maps input of the first query to output of the second.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IQuery<T1, T3> Connect<T1, T2, T3>(this IQuery<T1, T2> query, IQuery<T2, T3> other) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new ChainedQuery<T1, T2, T3>(query, other);
  }

  /// <summary>
  /// Connects a synchronous query to an asynchronous query.
  /// </summary>
  /// <returns>Asynchronous chained query.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncQuery<T1, T3> Connect<T1, T2, T3>(this IQuery<T1, T2> query, IAsyncQuery<T2, T3> other) {
    return query.ToAsyncQuery().Connect(other);
  }

  /// <summary>
  /// Converts a synchronous query into an asynchronous query.
  /// </summary>
  /// <returns>Asynchronous query proxy.</returns>
  [Pure]
  public static IAsyncQuery<T1, T2> ToAsyncQuery<T1, T2>(this IQuery<T1, T2> query) {
    query.ThrowIfNullReference();
    return new AsyncProxyQuery<T1, T2>(query);
  }

}