using System.Diagnostics;

namespace Executables.Core.Executors;

internal sealed class ThrottleExecutor<T>(IExecutor<T, Unit> executor, TimeSpan interval) : IExecutor<T, Unit> {

  private readonly Stopwatch _stopwatch = new();
  private readonly object _lock = new();

  Unit IExecutor<T, Unit>.Execute(T input) {
    lock (_lock) {
      if (_stopwatch.IsRunning && _stopwatch.Elapsed < interval)
        return default;
      _stopwatch.Restart();
    }

    executor.Execute(input);
    return default;
  }

}