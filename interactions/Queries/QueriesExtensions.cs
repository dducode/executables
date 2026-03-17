using Interactions.Context;
using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.Queries;

public static partial class QueriesExtensions {

  public static T2 Send<T1, T2>(this IQuery<T1, T2> query, T1 request, ContextInit init) {
    ExceptionsHelper.ThrowIfNull(init, nameof(init));

    IReadonlyContext previous = InteractionContext.Current;
    using var current = new InteractionContext(previous);
    init(new ContextWriter(current));
    InteractionContext.Current = current;

    try {
      return query.Send(request);
    }
    finally {
      InteractionContext.Current = previous;
    }
  }

  public static T Send<T>(this IQuery<Unit, T> query, ContextInit init) {
    return query.Send(default, init);
  }

  public static void Send(this IQuery<Unit, Unit> query, ContextInit init) {
    query.Send(default, init);
  }

}