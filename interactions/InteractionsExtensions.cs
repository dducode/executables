using Interactions.Core.Executables;
using Interactions.Core.Internal;

namespace Interactions;

public static partial class InteractionsExtensions {

  public static T2 Execute<T1, T2>(this IExecutable<T1, T2> executable, T1 input, Action<InteractionContext> init) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    ExceptionsHelper.ThrowIfNull(init, nameof(init));

    IReadonlyContext previous = InteractionContext.Current;
    using var current = new InteractionContext(previous, Guid.NewGuid());
    init(current);
    InteractionContext.Current = current;

    try {
      return executable.Execute(input);
    }
    finally {
      InteractionContext.Current = previous;
    }
  }

  public static T4 Invoke<T1, T2, T3, T4>(
    this ExecutionOperator<T1, T2, T3, T4> executionOperator,
    T1 input,
    IExecutable<T2, T3> executable,
    Action<InteractionContext> init) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    ExceptionsHelper.ThrowIfNull(init, nameof(init));

    IReadonlyContext previous = InteractionContext.Current;
    using var current = new InteractionContext(previous, Guid.NewGuid());
    init(current);
    InteractionContext.Current = current;

    try {
      return executionOperator.Invoke(input, executable);
    }
    finally {
      InteractionContext.Current = previous;
    }
  }

}