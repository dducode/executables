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

}