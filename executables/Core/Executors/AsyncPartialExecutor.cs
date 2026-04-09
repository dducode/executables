namespace Executables.Core.Executors;

internal sealed class ThreeArgsAsyncPartialExecutor<T1, T2, T3, T4, T5>(IAsyncExecutor<(T1, T2, T3, T4), T5> executor, T1 t1, T2 t2, T3 t3)
  : IAsyncExecutor<T4, T5> {

  ValueTask<T5> IAsyncExecutor<T4, T5>.Execute(T4 input, CancellationToken token) {
    return executor.Execute(t1, t2, t3, input, token);
  }

}

internal sealed class TwoArgsAsyncPartialExecutor<T1, T2, T3, T4, T5>(IAsyncExecutor<(T1, T2, T3, T4), T5> executor, T1 t1, T2 t2)
  : IAsyncExecutor<(T3, T4), T5> {

  ValueTask<T5> IAsyncExecutor<(T3, T4), T5>.Execute((T3, T4) input, CancellationToken token) {
    return executor.Execute(t1, t2, input.Item1, input.Item2, token);
  }

}

internal sealed class OneArgAsyncPartialExecutor<T1, T2, T3, T4, T5>(IAsyncExecutor<(T1, T2, T3, T4), T5> executor, T1 t1) : IAsyncExecutor<(T2, T3, T4), T5> {

  ValueTask<T5> IAsyncExecutor<(T2, T3, T4), T5>.Execute((T2, T3, T4) input, CancellationToken token) {
    return executor.Execute(t1, input.Item1, input.Item2, input.Item3, token);
  }

}

internal sealed class TwoArgsAsyncPartialExecutor<T1, T2, T3, T4>(IAsyncExecutor<(T1, T2, T3), T4> executor, T1 t1, T2 t2) : IAsyncExecutor<T3, T4> {

  ValueTask<T4> IAsyncExecutor<T3, T4>.Execute(T3 input, CancellationToken token) {
    return executor.Execute(t1, t2, input, token);
  }

}

internal sealed class OneArgAsyncPartialExecutor<T1, T2, T3, T4>(IAsyncExecutor<(T1, T2, T3), T4> executor, T1 t1) : IAsyncExecutor<(T2, T3), T4> {

  ValueTask<T4> IAsyncExecutor<(T2, T3), T4>.Execute((T2, T3) input, CancellationToken token) {
    return executor.Execute(t1, input.Item1, input.Item2, token);
  }

}

internal sealed class AsyncPartialExecutor<T1, T2, T3>(IAsyncExecutor<(T1, T2), T3> executor, T1 t1) : IAsyncExecutor<T2, T3> {

  ValueTask<T3> IAsyncExecutor<T2, T3>.Execute(T2 input, CancellationToken token) {
    return executor.Execute(t1, input, token);
  }

}