namespace Interactions.Core.Executables;

internal sealed class AsyncAnonymousExecutable<T1, T2>(AsyncFunc<T1, T2> func) : IAsyncExecutable<T1, T2> {

  public ValueTask<T2> Execute(T1 input, CancellationToken token = default) {
    return func(input, token);
  }

}