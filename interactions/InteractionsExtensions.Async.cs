using Interactions.Core;

namespace Interactions;

public static partial class InteractionsExtensions {

  public static ValueTask<T2> Execute<T1, T2>(
    this IAsyncExecutable<T1, T2> executable,
    T1 input,
    Action<InteractionContext> init,
    CancellationToken token = default) {
    return InvokeWithContext(input, executable, init, token);
  }

  public static ValueTask<T4> Invoke<T1, T2, T3, T4>(
    this AsyncExecutionOperator<T1, T2, T3, T4> executionOperator,
    T1 input,
    IAsyncExecutable<T2, T3> executable,
    Action<InteractionContext> init,
    CancellationToken token = default) {
    return InvokeWithContext(input, executionOperator.AsExecutable(executable), init, token);
  }

  private static async ValueTask<T2> InvokeWithContext<T1, T2>(
    T1 input,
    IAsyncExecutable<T1, T2> executable,
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

}