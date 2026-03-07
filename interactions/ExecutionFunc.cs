using Interactions.Core.Executables;

namespace Interactions;

public delegate T4 ExecutionFunc<in T1, out T2, in T3, out T4>(T1 input, IExecutable<T2, T3> next);

public delegate ValueTask<T4> AsyncExecutionFunc<in T1, out T2, T3, T4>(T1 input, IAsyncExecutable<T2, T3> next, CancellationToken token = default);