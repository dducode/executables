using Interactions.Core;
using Interactions.Core.Commands;
using Interactions.Core.Extensions;
using Interactions.Core.Queries;
using Interactions.Policies;

namespace Interactions;

public static partial class InteractionsExtensions {

  public static ValueTask<T2> Send<T1, T2>(this IAsyncQuery<T1, T2> query, T1 input, Action<InteractionContext> init, CancellationToken token = default) {
    return InvokeWithContext(input, query.Send, init, token);
  }

  public static ValueTask<T> Send<T>(this IAsyncQuery<Unit, T> query, Action<InteractionContext> init, CancellationToken token = default) {
    return InvokeWithContext(query.Send, init, token);
  }

  public static ValueTask<Result<T2>> TrySend<T1, T2>(
    this IAsyncQuery<T1, T2> query,
    T1 input,
    Action<InteractionContext> init,
    CancellationToken token = default) {
    return InvokeWithContext(input, query.TrySend, init, token);
  }

  public static ValueTask<Result<T>> TrySend<T>(this IAsyncQuery<Unit, T> query, Action<InteractionContext> init, CancellationToken token = default) {
    return InvokeWithContext(query.TrySend, init, token);
  }

  public static ValueTask<bool> Execute<T>(this IAsyncCommand<T> command, T input, Action<InteractionContext> init, CancellationToken token = default) {
    return InvokeWithContext(input, command.Execute, init, token);
  }

  public static ValueTask<bool> Execute(this IAsyncCommand<Unit> command, Action<InteractionContext> init, CancellationToken token = default) {
    return InvokeWithContext(command.Execute, init, token);
  }

  public static ValueTask<T2> Handle<T1, T2>(this AsyncHandler<T1, T2> handler, T1 input, Action<InteractionContext> init, CancellationToken token = default) {
    return InvokeWithContext(input, handler.Handle, init, token);
  }

  public static ValueTask<T2> Execute<T1, T2>(
    this AsyncPolicy<T1, T2> policy,
    T1 input,
    AsyncFunc<T1, T2> invocation,
    Action<InteractionContext> init,
    CancellationToken token = default) {
    return InvokeWithContext(input, (i, t) => policy.Execute(i, invocation, t), init, token);
  }

  private static async ValueTask<T2> InvokeWithContext<T1, T2>(
    T1 input,
    AsyncFunc<T1, T2> invocation,
    Action<InteractionContext> init,
    CancellationToken token = default) {
    ExceptionsHelper.ThrowIfNull(invocation, nameof(invocation));
    ExceptionsHelper.ThrowIfNull(init, nameof(init));

    IReadonlyContext previous = InteractionContext.Current;
    using var current = new InteractionContext(previous, Guid.NewGuid());
    init(current);
    InteractionContext.Current = current;

    try {
      return await invocation(input, token);
    }
    finally {
      InteractionContext.Current = previous;
    }
  }

  private static ValueTask<T> InvokeWithContext<T>(AsyncFunc<T> invocation, Action<InteractionContext> init, CancellationToken token = default) {
    return InvokeWithContext(default(Unit), (_, t) => invocation(t), init, token);
  }

}