namespace Interactions.Context;

public delegate T ContextFunc<out T>(ContextInit init);

public delegate T2 ContextFunc<in T1, out T2>(T1 arg, ContextInit init);