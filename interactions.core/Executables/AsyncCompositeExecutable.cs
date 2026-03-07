namespace Interactions.Core.Executables;

internal sealed class AsyncCompositeExecutable<T1, T2, T3>(IAsyncExecutable<T1, T2> first, IAsyncExecutable<T2, T3> second) : IAsyncExecutable<T1, T3> {

  public async ValueTask<T3> Execute(T1 input, CancellationToken token = default) {
    return await second.Execute(await first.Execute(input, token), token);
  }

}