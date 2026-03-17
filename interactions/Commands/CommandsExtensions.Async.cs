using Interactions.Context;
using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.Commands;

public static partial class CommandsExtensions {

  public static async ValueTask<bool> Execute<T>(this IAsyncCommand<T> command, T input, ContextInit init, CancellationToken token = default) {
    ExceptionsHelper.ThrowIfNull(init, nameof(init));

    IReadonlyContext previous = InteractionContext.Current;
    using var current = new InteractionContext(previous);
    init(new ContextWriter(current));
    InteractionContext.Current = current;

    try {
      return await command.Execute(input, token);
    }
    finally {
      InteractionContext.Current = previous;
    }
  }

  public static ValueTask<bool> Execute(this IAsyncCommand<Unit> command, ContextInit init, CancellationToken token = default) {
    return command.Execute(default, init, token: token);
  }

}