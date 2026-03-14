using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Core.Queries;

public static partial class QueriesExtensions {

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
    return query.TrySend(default);
  }

  [Pure]
  public static IAsyncQuery<T1, T2> ToAsyncQuery<T1, T2>(this IQuery<T1, T2> query) {
    query.ThrowIfNullReference();
    return new AsyncProxyQuery<T1, T2>(query);
  }

}