namespace Executables;

/// <summary>
/// Represents an asynchronous function with four input arguments and a result.
/// </summary>
/// <typeparam name="T1">Type of the first input argument.</typeparam>
/// <typeparam name="T2">Type of the second input argument.</typeparam>
/// <typeparam name="T3">Type of the third input argument.</typeparam>
/// <typeparam name="T4">Type of the fourth input argument.</typeparam>
/// <typeparam name="TResult">Type of the function result.</typeparam>
/// <param name="arg1">First input argument.</param>
/// <param name="arg2">Second input argument.</param>
/// <param name="arg3">Third input argument.</param>
/// <param name="arg4">Fourth input argument.</param>
/// <param name="token">Cancellation token.</param>
/// <returns>Asynchronous operation producing a result.</returns>
public delegate ValueTask<TResult> AsyncFunc<in T1, in T2, in T3, in T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken token = default);

/// <summary>
/// Represents an asynchronous function with three input arguments and a result.
/// </summary>
/// <typeparam name="T1">Type of the first input argument.</typeparam>
/// <typeparam name="T2">Type of the second input argument.</typeparam>
/// <typeparam name="T3">Type of the third input argument.</typeparam>
/// <typeparam name="TResult">Type of the function result.</typeparam>
/// <param name="arg1">First input argument.</param>
/// <param name="arg2">Second input argument.</param>
/// <param name="arg3">Third input argument.</param>
/// <param name="token">Cancellation token.</param>
/// <returns>Asynchronous operation producing a result.</returns>
public delegate ValueTask<TResult> AsyncFunc<in T1, in T2, in T3, TResult>(T1 arg1, T2 arg2, T3 arg3, CancellationToken token = default);

/// <summary>
/// Represents an asynchronous function with two input arguments and a result.
/// </summary>
/// <typeparam name="T1">Type of the first input argument.</typeparam>
/// <typeparam name="T2">Type of the second input argument.</typeparam>
/// <typeparam name="TResult">Type of the function result.</typeparam>
/// <param name="arg1">First input argument.</param>
/// <param name="arg2">Second input argument.</param>
/// <param name="token">Cancellation token.</param>
/// <returns>Asynchronous operation producing a result.</returns>
public delegate ValueTask<TResult> AsyncFunc<in T1, in T2, TResult>(T1 arg1, T2 arg2, CancellationToken token = default);

/// <summary>
/// Represents an asynchronous function with one input argument and a result.
/// </summary>
/// <typeparam name="T1">Type of the input argument.</typeparam>
/// <typeparam name="TResult">Type of the function result.</typeparam>
/// <param name="arg">Input argument.</param>
/// <param name="token">Cancellation token.</param>
/// <returns>Asynchronous operation producing a result.</returns>
public delegate ValueTask<TResult> AsyncFunc<in T1, TResult>(T1 arg, CancellationToken token = default);

/// <summary>
/// Represents an asynchronous function with no input arguments and a result.
/// </summary>
/// <typeparam name="TResult">Type of the function result.</typeparam>
/// <param name="token">Cancellation token.</param>
/// <returns>Asynchronous operation producing a result.</returns>
public delegate ValueTask<TResult> AsyncFunc<TResult>(CancellationToken token = default);