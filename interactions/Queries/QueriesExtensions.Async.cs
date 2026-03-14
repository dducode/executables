using Interactions.Context;
using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.Queries;

public static partial class QueriesExtensions {

  public static async ValueTask<T2> Send<T1, T2>(
    this IAsyncQuery<T1, T2> query,
    T1 request,
    Action<InteractionContext> init,
    CancellationToken token = default) {
    ExceptionsHelper.ThrowIfNull(init, nameof(init));

    IReadonlyContext previous = InteractionContext.Current;
    using var current = new InteractionContext(previous);
    init(current);
    InteractionContext.Current = current;

    try {
      return await query.Send(request, token);
    }
    finally {
      InteractionContext.Current = previous;
    }
  }

  public static ValueTask<T> Send<T>(this IAsyncQuery<Unit, T> query, Action<InteractionContext> init, CancellationToken token = default) {
    return query.Send(default, init, token: token);
  }

  public static async ValueTask Send(this IAsyncQuery<Unit, Unit> query, Action<InteractionContext> init, CancellationToken token = default) {
    await query.Send(default, init, token: token);
  }

}