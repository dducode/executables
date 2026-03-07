using Interactions.Core;

namespace Interactions;

public static partial class InteractionsExtensions {

  public static async ValueTask<T2> Execute<T1, T2>(
    this IAsyncExecutable<T1, T2> executable,
    T1 input,
    Action<InteractionContext> init,
    CancellationToken token = default) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    ExceptionsHelper.ThrowIfNull(init, nameof(init));

    IReadonlyContext previous = InteractionContext.Current;
    using var current = new InteractionContext(previous, Guid.NewGuid());
    init(current);
    InteractionContext.Current = current;

    try {
      return await executable.Execute(input, token);
    }
    finally {
      InteractionContext.Current = previous;
    }
  }

  public static async ValueTask<T4> Invoke<T1, T2, T3, T4>(
    this AsyncExecutionOperator<T1, T2, T3, T4> executionOperator,
    T1 input,
    IAsyncExecutable<T2, T3> executable,
    Action<InteractionContext> init,
    CancellationToken token = default) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    ExceptionsHelper.ThrowIfNull(init, nameof(init));

    IReadonlyContext previous = InteractionContext.Current;
    using var current = new InteractionContext(previous, Guid.NewGuid());
    init(current);
    InteractionContext.Current = current;

    try {
      return await executionOperator.Invoke(input, executable, token);
    }
    finally {
      InteractionContext.Current = previous;
    }
  }

}