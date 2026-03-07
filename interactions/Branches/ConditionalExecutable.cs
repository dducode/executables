using Interactions.Core;

namespace Interactions.Branches;

internal sealed class ConditionalExecutable<T1, T2>(
  Func<bool> condition,
  IExecutable<T1, T2> ifExecutable,
  IExecutable<T1, T2> elseExecutable) : IExecutable<T1, T2> {

  public T2 Execute(T1 input) {
    return condition() ? ifExecutable.Execute(input) : elseExecutable.Execute(input);
  }

}