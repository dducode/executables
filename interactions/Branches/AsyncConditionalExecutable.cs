using Interactions.Core;

namespace Interactions.Branches;

internal sealed class AsyncConditionalExecutable<T1, T2>(
  Func<bool> condition,
  IAsyncExecutable<T1, T2> ifExecutable,
  IAsyncExecutable<T1, T2> elseExecutable) : IAsyncExecutable<T1, T2> {

  public ValueTask<T2> Execute(T1 input, CancellationToken token = default) {
    return condition() ? ifExecutable.Execute(input, token) : elseExecutable.Execute(input, token);
  }

}