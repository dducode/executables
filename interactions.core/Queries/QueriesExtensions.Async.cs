namespace Interactions.Core.Queries;

public static partial class QueriesExtensions {

  public static ValueTask<T> Send<T>(this IAsyncQuery<Unit, T> query, CancellationToken token = default) {
    return query.Send(default, token);
  }

  public static async ValueTask Send(this IAsyncQuery<Unit, Unit> query, CancellationToken token = default) {
    await query.Send(default, token);
  }

  public static async ValueTask<Result<T2>> TrySend<T1, T2>(this IAsyncQuery<T1, T2> query, T1 request, CancellationToken token = default) {
    try {
      return await query.Send(request, token);
    }
    catch (Exception e) when (e is MissingHandlerException or OperationCanceledException) {
      return Result<T2>.FromException(e);
    }
  }

  public static ValueTask<Result<T>> TrySend<T>(this IAsyncQuery<Unit, T> query, CancellationToken token = default) {
    return query.TrySend(default, token);
  }

}