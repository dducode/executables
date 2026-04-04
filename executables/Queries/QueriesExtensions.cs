using System.Diagnostics.Contracts;
using Executables.Core.Queries;
using Executables.Internal;

namespace Executables.Queries;

public static class QueriesExtensions {

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
  /// Converts a synchronous query into an asynchronous query.
  /// </summary>
  /// <returns>Asynchronous query proxy.</returns>
  [Pure]
  public static IAsyncQuery<T1, T2> ToAsyncQuery<T1, T2>(this IQuery<T1, T2> query) {
    query.ThrowIfNullReference();
    return new AsyncProxyQuery<T1, T2>(query);
  }

}