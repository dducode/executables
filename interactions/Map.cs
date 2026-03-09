using Interactions.Core;

namespace Interactions;

public class Map<T1, T2, T3, T4>(Transformer<T1, T2> incoming, Transformer<T3, T4> outgoing) : ExecutionOperator<T1, T2, T3, T4> {

  internal static Map<T1, T1, T2, T2> Identity { get; } = new(Transformer.Identity<T1>(), Transformer.Identity<T2>());

  public override T4 Invoke(T1 input, IExecutable<T2, T3> next) {
    return outgoing.Transform(next.Execute(incoming.Transform(input)));
  }

}