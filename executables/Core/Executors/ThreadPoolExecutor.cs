namespace Executables.Core.Executors;

internal sealed class ThreadPoolExecutor<T>(IExecutor<T, Unit> executor) : IExecutor<T, Unit> {

  Unit IExecutor<T, Unit>.Execute(T input) {
    ThreadPool.QueueUserWorkItem(_ => executor.Execute(input));
    return default;
  }

}