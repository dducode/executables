namespace Interactions.Context;

public delegate void ContextAction(ContextInit init);

public delegate void ContextAction<in T>(T arg, ContextInit init);