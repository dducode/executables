using Interactions.Policies;

namespace Interactions.Core.Policies;

internal sealed class ThreadPoolPolicy<T> : Policy<T, Unit> {

  public override Unit Invoke(T input, IExecutor<T, Unit> executor) {
    ThreadPool.QueueUserWorkItem(_ => executor.Execute(input));
    return default;
  }

}