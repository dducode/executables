namespace Interactions.Context;

public delegate ValueTask AsyncContextAction(ContextInit init, CancellationToken token = default);

public delegate ValueTask AsyncContextAction<in T>(T arg, ContextInit init, CancellationToken token = default);