using Interactions.Context;
using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.Events;

public static class EventsExtensions {

  public static void Publish<T>(this IEvent<T> e, T message, ContextInit init) {
    ExceptionsHelper.ThrowIfNull(init, nameof(init));

    IReadonlyContext previous = InteractionContext.Current;
    using var current = new InteractionContext(previous);
    init(new ContextWriter(current));
    InteractionContext.Current = current;

    try {
      e.Publish(message);
    }
    finally {
      InteractionContext.Current = previous;
    }
  }

  public static void Publish(this IEvent<Unit> e, ContextInit init) {
    e.Publish(default, init);
  }

}