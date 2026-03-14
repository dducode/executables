using Interactions.Core;

namespace Interactions;

public delegate T4 ExecutionFunc<in T1, out T2, in T3, out T4>(T1 input, IExecutor<T2, T3> executor);

public delegate ValueTask<T4> AsyncExecutionFunc<in T1, out T2, T3, T4>(T1 input, IAsyncExecutor<T2, T3> executor, CancellationToken token = default);