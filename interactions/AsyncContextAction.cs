namespace Interactions;

public delegate ValueTask AsyncContextAction(Action<InteractionContext> init, CancellationToken token = default);

public delegate ValueTask AsyncContextAction<in T>(T arg, Action<InteractionContext> init, CancellationToken token = default);