using Interactions.Context;
using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.Executables;

public static partial class ExecutableExtensions {

  public static T2 Execute<T1, T2>(this IExecutor<T1, T2> executor, T1 input, ContextInit init) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(init, nameof(init));

    IReadonlyContext previous = InteractionContext.Current;
    using var current = new InteractionContext(previous);
    init(new ContextWriter(current));
    InteractionContext.Current = current;

    try {
      return executor.Execute(input);
    }
    finally {
      InteractionContext.Current = previous;
    }
  }

  public static void Execute(this IExecutor<Unit, Unit> executor, ContextInit init) {
    executor.Execute(default, init);
  }

  public static T Execute<T>(this IExecutor<Unit, T> executor, ContextInit init) {
    return executor.Execute(default, init);
  }

}