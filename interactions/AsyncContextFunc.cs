namespace Interactions;

public delegate ValueTask<T> AsyncContextFunc<T>(Action<InteractionContext> init, CancellationToken token = default);

public delegate ValueTask<T2> AsyncContextFunc<in T1, T2>(T1 arg, Action<InteractionContext> init, CancellationToken token = default);