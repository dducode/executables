using Executables.Operations;

namespace Executables.Policies;

/// <summary>
/// Defines an asynchronous policy that can wrap query or command invocation.
/// </summary>
/// <typeparam name="T1">Input value type.</typeparam>
/// <typeparam name="T2">Result value type.</typeparam>
public abstract class AsyncPolicy<T1, T2> : AsyncBehaviorOperator<T1, T2>;