using Interactions.Operations;

namespace Interactions;

/// <summary>
/// Represents one middleware step in an asynchronous pipeline.
/// </summary>
/// <typeparam name="T1">Input type accepted by the current step.</typeparam>
/// <typeparam name="T2">Input type expected by the downstream handler.</typeparam>
/// <typeparam name="T3">Output type returned by the downstream handler.</typeparam>
/// <typeparam name="T4">Output type returned by the current step.</typeparam>
public abstract class AsyncMiddleware<T1, T2, T3, T4> : AsyncExecutionOperator<T1, T2, T3, T4>;