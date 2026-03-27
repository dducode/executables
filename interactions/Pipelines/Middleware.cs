using Interactions.Operations;

namespace Interactions.Pipelines;

/// <summary>
/// Represents one middleware step in a synchronous pipeline.
/// </summary>
/// <typeparam name="T1">Input type accepted by the current step.</typeparam>
/// <typeparam name="T2">Input type expected by the downstream handler.</typeparam>
/// <typeparam name="T3">Output type returned by the downstream handler.</typeparam>
/// <typeparam name="T4">Output type returned by the current step.</typeparam>
public abstract class Middleware<T1, T2, T3, T4> : ExecutionOperator<T1, T2, T3, T4>;