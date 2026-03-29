namespace Interactions;

/// <summary>
/// Represents an asynchronous action with four input arguments.
/// </summary>
/// <typeparam name="T1">Type of the first input argument.</typeparam>
/// <typeparam name="T2">Type of the second input argument.</typeparam>
/// <typeparam name="T3">Type of the third input argument.</typeparam>
/// <typeparam name="T4">Type of the fourth input argument.</typeparam>
/// <param name="arg1">First input argument.</param>
/// <param name="arg2">Second input argument.</param>
/// <param name="arg3">Third input argument.</param>
/// <param name="arg4">Fourth input argument.</param>
/// <param name="token">Cancellation token.</param>
/// <returns>Asynchronous operation.</returns>
public delegate ValueTask AsyncAction<in T1, in T2, in T3, in T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken token = default);

/// <summary>
/// Represents an asynchronous action with three input arguments.
/// </summary>
/// <typeparam name="T1">Type of the first input argument.</typeparam>
/// <typeparam name="T2">Type of the second input argument.</typeparam>
/// <typeparam name="T3">Type of the third input argument.</typeparam>
/// <param name="arg1">First input argument.</param>
/// <param name="arg2">Second input argument.</param>
/// <param name="arg3">Third input argument.</param>
/// <param name="token">Cancellation token.</param>
/// <returns>Asynchronous operation.</returns>
public delegate ValueTask AsyncAction<in T1, in T2, in T3>(T1 arg1, T2 arg2, T3 arg3, CancellationToken token = default);

/// <summary>
/// Represents an asynchronous action with two input arguments.
/// </summary>
/// <typeparam name="T1">Type of the first input argument.</typeparam>
/// <typeparam name="T2">Type of the second input argument.</typeparam>
/// <param name="arg1">First input argument.</param>
/// <param name="arg2">Second input argument.</param>
/// <param name="token">Cancellation token.</param>
/// <returns>Asynchronous operation.</returns>
public delegate ValueTask AsyncAction<in T1, in T2>(T1 arg1, T2 arg2, CancellationToken token = default);

/// <summary>
/// Represents an asynchronous action with one input argument.
/// </summary>
/// <typeparam name="T">Type of the input argument.</typeparam>
/// <param name="arg">Input argument.</param>
/// <param name="token">Cancellation token.</param>
/// <returns>Asynchronous operation.</returns>
public delegate ValueTask AsyncAction<in T>(T arg, CancellationToken token = default);

/// <summary>
/// Represents an asynchronous action with no input arguments.
/// </summary>
/// <param name="token">Cancellation token.</param>
/// <returns>Asynchronous operation.</returns>
public delegate ValueTask AsyncAction(CancellationToken token = default);