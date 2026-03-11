using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.Context;

public static partial class InteractionsExtensions {

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

}