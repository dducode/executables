using Interactions.Context;
using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.Executables;

public static partial class ExecutableExtensions {

  public static async ValueTask<T2> Execute<T1, T2>(
    this IAsyncExecutable<T1, T2> executable,
    T1 input,
    Action<InteractionContext> init,
    CancellationToken token = default) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(init, nameof(init));

    IReadonlyContext previous = InteractionContext.Current;
    using var current = new InteractionContext(previous);
    init(current);
    InteractionContext.Current = current;

    try {
      return await executable.Execute(input, token);
    }
    finally {
      InteractionContext.Current = previous;
    }
  }

  public static ValueTask<T> Execute<T>(this IAsyncExecutable<Unit, T> executable, Action<InteractionContext> init, CancellationToken token = default) {
    return executable.Execute(default, init, token);
  }

  public static async ValueTask Execute(this IAsyncExecutable<Unit, Unit> executable, Action<InteractionContext> init, CancellationToken token = default) {
    await executable.Execute(default, init, token);
  }

}