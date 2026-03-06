using Interactions.Core;
using Interactions.Core.Commands;
using Interactions.Core.Events;
using Interactions.Core.Extensions;
using Interactions.Core.Queries;
using Interactions.Policies;
using Interactions.Transformation;
using Interactions.Validation;

namespace Interactions;

public static partial class InteractionsExtensions {

  public static T2 Send<T1, T2>(this IQuery<T1, T2> query, T1 input, Action<InteractionContext> init) {
    return InvokeWithContext(input, query.Send, init);
  }

  public static T Send<T>(this IQuery<Unit, T> query, Action<InteractionContext> init) {
    return InvokeWithContext(query.Send, init);
  }

  public static Result<T2> TrySend<T1, T2>(this IQuery<T1, T2> query, T1 input, Action<InteractionContext> init) {
    return InvokeWithContext(input, query.TrySend, init);
  }

  public static Result<T> TrySend<T>(this IQuery<Unit, T> query, Action<InteractionContext> init) {
    return InvokeWithContext(query.TrySend, init);
  }

  public static bool Execute<T>(this ICommand<T> command, T input, Action<InteractionContext> init) {
    return InvokeWithContext(input, command.Execute, init);
  }

  public static bool Execute(this ICommand<Unit> command, Action<InteractionContext> init) {
    return InvokeWithContext(command.Execute, init);
  }

  public static void Publish<T>(this IEvent<T> e, T input, Action<InteractionContext> init) {
    InvokeWithContext(input, e.Publish, init);
  }

  public static T2 Handle<T1, T2>(this Handler<T1, T2> handler, T1 input, Action<InteractionContext> init) {
    return InvokeWithContext(input, handler.Handle, init);
  }

  public static T2 Execute<T1, T2>(this Policy<T1, T2> policy, T1 input, Func<T1, T2> invocation, Action<InteractionContext> init) {
    return InvokeWithContext(input, i => policy.Execute(i, invocation), init);
  }

  public static bool IsValid<T>(this Validator<T> validator, T input, Action<InteractionContext> init) {
    return InvokeWithContext(input, validator.IsValid, init);
  }

  public static T2 Transform<T1, T2>(this Transformer<T1, T2> transformer, T1 input, Action<InteractionContext> init) {
    return InvokeWithContext(input, transformer.Transform, init);
  }

  private static T2 InvokeWithContext<T1, T2>(T1 input, Func<T1, T2> invocation, Action<InteractionContext> init) {
    ExceptionsHelper.ThrowIfNull(invocation, nameof(invocation));
    ExceptionsHelper.ThrowIfNull(init, nameof(init));

    IReadonlyContext previous = InteractionContext.Current;
    using var current = new InteractionContext(previous, Guid.NewGuid());
    init(current);
    InteractionContext.Current = current;

    try {
      return invocation(input);
    }
    finally {
      InteractionContext.Current = previous;
    }
  }

  private static T InvokeWithContext<T>(Func<T> invocation, Action<InteractionContext> init) {
    return InvokeWithContext(default(Unit), _ => invocation(), init);
  }

  private static void InvokeWithContext<T>(T input, Action<T> invocation, Action<InteractionContext> init) {
    InvokeWithContext(input, Unit (t) => {
      invocation(t);
      return default;
    }, init);
  }

}