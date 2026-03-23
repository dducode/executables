namespace Interactions;

/// <summary>
/// Represents a synchronous execution operator that preserves input and output contracts of the wrapped executor.
/// </summary>
/// <typeparam name="T1">Input type passed to the operator and downstream executor.</typeparam>
/// <typeparam name="T2">Result type returned by the downstream executor and the operator.</typeparam>
public abstract class BehaviorOperator<T1, T2> : ExecutionOperator<T1, T1, T2, T2>;