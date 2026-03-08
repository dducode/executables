namespace Interactions;

/// <summary>
/// Defines a synchronous policy that can wrap executable invocation.
/// </summary>
/// <typeparam name="T1">Input value type.</typeparam>
/// <typeparam name="T2">Result value type.</typeparam>
public abstract partial class Policy<T1, T2> : ExecutionOperator<T1, T1, T2, T2>;