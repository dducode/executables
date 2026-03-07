using Interactions.Core;
using Interactions.Core.Executables;

namespace Interactions;

internal sealed class AnonymousOperator<T1, T2, T3, T4>(ExecutionFunc<T1, T2, T3, T4> operation) : ExecutionOperator<T1, T2, T3, T4> {

  public override T4 Invoke(T1 input, IExecutable<T2, T3> next) {
    return operation(input, next);
  }

}