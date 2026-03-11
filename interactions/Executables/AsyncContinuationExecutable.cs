using Interactions.Core;

namespace Interactions.Executables;

internal sealed class AsyncContinuationExecutable<T1, T2>(IExecutable<T1, IAsyncExecutable<T1, T2>> continuation) : IAsyncExecutable<T1, T2> {

  public async ValueTask<T2> Execute(T1 input, CancellationToken token = default) {
    IAsyncExecutable<T1, T2> inner = continuation.Execute(input);
    return inner != null ? await inner.Execute(input, token) : throw new InvalidOperationException($"Cannot get executable from {continuation.GetType().Name}");
  }

}