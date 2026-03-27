using Interactions.Handling;
using Interactions.Internal;

namespace Interactions.Core.Commands;

internal sealed class AsyncCompositeCommand<T>(AsyncCommand<T> first, AsyncCommand<T> second) : AsyncCommand<T> {

  public override ValueTask<bool> Execute(T input, CancellationToken token = default) {
    ValueTask<bool> firstTask = first.Execute(input, token);

    if (firstTask.IsCompleted)
      return firstTask.Result ? second.Execute(input, token) : new ValueTask<bool>(false);

    return Await(input, firstTask, second, token);
  }

  private static async ValueTask<bool> Await(T input, ValueTask<bool> task, AsyncCommand<T> secondCommand, CancellationToken token) {
    return await task && await secondCommand.Execute(input, token);
  }

  public override IDisposable Handle(AsyncHandler<T, Unit> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return Handleable.MergedHandle(first, second, handler);
  }

}