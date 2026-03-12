using Interactions.Core;

namespace Interactions.Executables;

internal sealed class AsyncTerminateExecutable<T1, T2>(IAsyncExecutable<T1, T2> first, IAsyncExecutable<T2> second) : IAsyncExecutable<T1> {

  public async ValueTask Execute(T1 input, CancellationToken token = default) {
    await second.Execute(await first.Execute(input, token), token);
  }

}