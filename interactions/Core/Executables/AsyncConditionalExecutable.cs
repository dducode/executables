namespace Interactions.Core.Executables;

internal sealed class AsyncConditionalExecutable<T1, T2>(
  Func<T1, bool> condition,
  IAsyncExecutable<T1, T2> ifExecutable,
  IAsyncExecutable<T1, T2> elseExecutable) : IAsyncExecutable<T1, T2>, IAsyncExecutor<T1, T2> {

  private readonly IAsyncExecutor<T1, T2> _ifExecutor = ifExecutable.GetExecutor();
  private readonly IAsyncExecutor<T1, T2> _elseExecutor = elseExecutable.GetExecutor();

  IAsyncExecutor<T1, T2> IAsyncExecutable<T1, T2>.GetExecutor() {
    return this;
  }

  ValueTask<T2> IAsyncExecutor<T1, T2>.Execute(T1 input, CancellationToken token) {
    return condition(input) ? _ifExecutor.Execute(input, token) : _elseExecutor.Execute(input, token);
  }

}