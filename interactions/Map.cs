using Interactions.Core;

namespace Interactions;

public class Map<T1, T2, T3, T4>(IExecutable<T1, T2> incoming, IExecutable<T3, T4> outgoing) : ExecutionOperator<T1, T2, T3, T4> {

  internal static Map<T1, T1, T2, T2> Identity { get; } = new(Executable.Identity<T1>(), Executable.Identity<T2>());

  public override T4 Invoke(T1 input, IExecutable<T2, T3> next) {
    return outgoing.Execute(next.Execute(incoming.Execute(input)));
  }

}