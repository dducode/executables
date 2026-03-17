using Interactions.Context;
using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.Executables;

public static partial class ExecutableExtensions {

  public static async ValueTask<T2> Execute<T1, T2>(
    this IAsyncExecutor<T1, T2> executable,
    T1 input,
    ContextInit init,
    CancellationToken token = default) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(init, nameof(init));

    IReadonlyContext previous = InteractionContext.Current;
    using var current = new InteractionContext(previous);
    init(new ContextWriter(current));
    InteractionContext.Current = current;

    try {
      return await executable.Execute(input, token);
    }
    finally {
      InteractionContext.Current = previous;
    }
  }

  public static ValueTask<T> Execute<T>(this IAsyncExecutor<Unit, T> executable, ContextInit init, CancellationToken token = default) {
    return executable.Execute(default, init, token);
  }

  public static async ValueTask Execute(this IAsyncExecutor<Unit, Unit> executable, ContextInit init, CancellationToken token = default) {
    await executable.Execute(default, init, token);
  }

}