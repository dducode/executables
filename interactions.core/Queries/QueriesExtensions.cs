namespace Interactions.Core.Queries;

public static class QueriesExtensions {

  public static T Send<T>(this IQuery<Unit, T> query) {
    return query.Send(default);
  }

  public static void Send(this IQuery<Unit, Unit> query) {
    query.Send(default);
  }

  public static Result<T2> TrySend<T1, T2>(this IQuery<T1, T2> query, T1 request) {
    try {
      return query.Send(request);
    }
    catch (MissingHandlerException e) {
      return Result<T2>.FromException(e);
    }
  }

  public static Result<T> TrySend<T>(this IQuery<Unit, T> query) {
    try {
      return query.Send(default);
    }
    catch (MissingHandlerException e) {
      return Result<T>.FromException(e);
    }
  }

}