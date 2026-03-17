namespace Interactions.Context;

public delegate ValueTask<T> AsyncContextFunc<T>(ContextInit init, CancellationToken token = default);

public delegate ValueTask<T2> AsyncContextFunc<in T1, T2>(T1 arg, ContextInit init, CancellationToken token = default);