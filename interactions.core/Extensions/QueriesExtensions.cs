using System.Diagnostics.Contracts;
using Interactions.Core.Queries;

namespace Interactions.Core.Extensions;

public static class QueriesExtensions {

  public static T Send<T>(this IQuery<Unit, T> query) {
    return query.Send(default);
  }

  public static Result<T2> TrySend<T1, T2>(this IQuery<T1, T2> query, T1 input) {
    try {
      return query.Send(input);
    }
    catch (MissingHandlerException e) {
      return Result<T2>.FromException(e);
    }
  }

  public static Result<T> TrySend<T>(this IQuery<Unit, T> query) {
    return query.TrySend(default);
  }

  public static ValueTask<T> Send<T>(this IAsyncQuery<Unit, T> query, CancellationToken token = default) {
    return query.Send(default, token);
  }

  public static async ValueTask<Result<T2>> TrySend<T1, T2>(this IAsyncQuery<T1, T2> query, T1 input, CancellationToken token = default) {
    try {
      return await query.Send(input, token);
    }
    catch (Exception e) when (e is MissingHandlerException or OperationCanceledException) {
      return Result<T2>.FromException(e);
    }
  }

  public static async ValueTask<Result<T>> TrySend<T>(this IAsyncQuery<Unit, T> query, CancellationToken token = default) {
    return await query.TrySend(default, token);
  }

  [Pure]
  public static IAsyncQuery<T1, T2> ToAsyncQuery<T1, T2>(this IQuery<T1, T2> query) {
    ExceptionsHelper.ThrowIfNullReference(query);
    return new AsyncProxyQuery<T1, T2>(query);
  }

}