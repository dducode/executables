using Interactions.Core;

namespace Interactions.Policies;

internal sealed class ThreadPoolPolicy<T> : Policy<T, Unit> {

  public override Unit Execute(T input, IExecutable<T, Unit> executable) {
    ThreadPool.QueueUserWorkItem(_ => executable.Execute(input));
    return default;
  }

}