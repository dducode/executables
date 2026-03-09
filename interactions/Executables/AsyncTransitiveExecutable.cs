using Interactions.Core;

namespace Interactions.Executables;

internal sealed class AsyncTransitiveExecutable<T>(AsyncAction<T> action) : IAsyncExecutable<T, T> {

  public async ValueTask<T> Execute(T input, CancellationToken token = default) {
    await action(input, token);
    return input;
  }

}