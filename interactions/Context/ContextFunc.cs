namespace Interactions.Context;

public delegate T ContextFunc<out T>(Action<InteractionContext> init);

public delegate T2 ContextFunc<in T1, out T2>(T1 arg, Action<InteractionContext> init);