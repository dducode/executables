namespace Interactions.Context;

public delegate void ContextAction(Action<InteractionContext> init);

public delegate void ContextAction<in T>(T arg, Action<InteractionContext> init);