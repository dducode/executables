namespace Interactions;

public delegate ValueTask<T5> AsyncFunc<in T1, in T2, in T3, in T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken token = default);

public delegate ValueTask<T4> AsyncFunc<in T1, in T2, in T3, T4>(T1 arg1, T2 arg2, T3 arg3, CancellationToken token = default);

public delegate ValueTask<T3> AsyncFunc<in T1, in T2, T3>(T1 arg1, T2 arg2, CancellationToken token = default);

public delegate ValueTask<T2> AsyncFunc<in T1, T2>(T1 arg, CancellationToken token = default);

public delegate ValueTask<T> AsyncFunc<T>(CancellationToken token = default);