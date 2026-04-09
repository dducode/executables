using Executables.Core.Pipelines;
using Executables.Internal;
using Executables.Pipelines;

namespace Executables;

/// <summary>
/// Entry point for building asynchronous middleware pipelines.
/// </summary>
public static class AsyncPipeline {

  /// <summary>
  /// Starts an async pipeline from a step that receives downstream call as async function.
  /// </summary>
  /// <typeparam name="T1">Input type accepted by the middleware.</typeparam>
  /// <typeparam name="T2">Input type expected by the downstream async function.</typeparam>
  /// <typeparam name="T3">Output type returned by the downstream async function.</typeparam>
  /// <typeparam name="T4">Output type returned by the middleware.</typeparam>
  /// <param name="middleware">Async middleware function that can invoke downstream function.</param>
  /// <returns>Builder for appending next middleware steps and terminal async executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="middleware"/> is <see langword="null"/>.</exception>
  public static AsyncPipelineBuilder<T1, T2, T3, T4> Use<T1, T2, T3, T4>(AsyncFunc<T1, AsyncFunc<T2, T3>, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<T1, T2, T3, T4>(new AsyncAnonymousMiddleware<T1, T2, T3, T4>((input, executable, token) =>
      middleware(input, executable.Execute, token)
    ));
  }

  /// <summary>
  /// Starts an async pipeline from a step that receives downstream parameterless async function.
  /// </summary>
  /// <typeparam name="T1">Input type accepted by the middleware.</typeparam>
  /// <typeparam name="T2">Output type returned by the downstream parameterless async function.</typeparam>
  /// <typeparam name="T3">Output type returned by the middleware.</typeparam>
  /// <param name="middleware">Async middleware function that can invoke downstream parameterless async function.</param>
  /// <returns>Builder with downstream input fixed to <see cref="Unit" />.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="middleware"/> is <see langword="null"/>.</exception>
  public static AsyncPipelineBuilder<T1, Unit, T2, T3> Use<T1, T2, T3>(AsyncFunc<T1, AsyncFunc<T2>, T3> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<T1, Unit, T2, T3>(new AsyncAnonymousMiddleware<T1, Unit, T2, T3>((input, executable, token) => {
      return middleware(input, t => executable.Execute(default, t), token);
    }));
  }

  /// <summary>
  /// Starts an async pipeline from a step that forwards via typed async action.
  /// </summary>
  /// <typeparam name="T1">Input type accepted by the middleware.</typeparam>
  /// <typeparam name="T2">Input type accepted by the downstream async action.</typeparam>
  /// <typeparam name="T3">Output type returned by the middleware.</typeparam>
  /// <param name="middleware">Async middleware function that calls downstream action.</param>
  /// <returns>Builder whose downstream output type is <see cref="Unit" />.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="middleware"/> is <see langword="null"/>.</exception>
  public static AsyncPipelineBuilder<T1, T2, Unit, T3> Use<T1, T2, T3>(AsyncFunc<T1, AsyncAction<T2>, T3> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<T1, T2, Unit, T3>(new AsyncAnonymousMiddleware<T1, T2, Unit, T3>((input, executable, token) => {
      return middleware(input, async (i, t) => await executable.Execute(i, t), token);
    }));
  }

  /// <summary>
  /// Starts an async pipeline from a step that forwards via parameterless async action.
  /// </summary>
  /// <typeparam name="T1">Input type accepted by the middleware.</typeparam>
  /// <typeparam name="T2">Output type returned by the middleware.</typeparam>
  /// <param name="middleware">Async middleware function that calls downstream action.</param>
  /// <returns>Builder whose downstream input and output types are <see cref="Unit" />.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="middleware"/> is <see langword="null"/>.</exception>
  public static AsyncPipelineBuilder<T1, Unit, Unit, T2> Use<T1, T2>(AsyncFunc<T1, AsyncAction, T2> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<T1, Unit, Unit, T2>(new AsyncAnonymousMiddleware<T1, Unit, Unit, T2>((input, executable, token) => {
      return middleware(input, async t => await executable.Execute(default, t), token);
    }));
  }

  /// <summary>
  /// Starts a parameterless async pipeline that returns value and can invoke downstream typed async function.
  /// </summary>
  /// <typeparam name="T1">Input type expected by the downstream async function.</typeparam>
  /// <typeparam name="T2">Output type returned by the downstream async function.</typeparam>
  /// <typeparam name="T3">Output type returned by the middleware.</typeparam>
  /// <param name="middleware">Async middleware function that can invoke downstream async function.</param>
  /// <returns>Builder for appending next middleware steps and terminal async executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="middleware"/> is <see langword="null"/>.</exception>
  public static AsyncPipelineBuilder<Unit, T1, T2, T3> Use<T1, T2, T3>(AsyncFunc<AsyncFunc<T1, T2>, T3> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<Unit, T1, T2, T3>(
      new AsyncAnonymousMiddleware<Unit, T1, T2, T3>((_, executable, token) => middleware(executable.Execute, token)));
  }

  /// <summary>
  /// Starts a parameterless async pipeline that returns value and can invoke downstream parameterless async function.
  /// </summary>
  /// <typeparam name="T1">Output type returned by the downstream parameterless async function.</typeparam>
  /// <typeparam name="T2">Output type returned by the middleware.</typeparam>
  /// <param name="middleware">Async middleware function that can invoke downstream parameterless async function.</param>
  /// <returns>Builder with downstream input fixed to <see cref="Unit" />.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="middleware"/> is <see langword="null"/>.</exception>
  public static AsyncPipelineBuilder<Unit, Unit, T1, T2> Use<T1, T2>(AsyncFunc<AsyncFunc<T1>, T2> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<Unit, Unit, T1, T2>(new AsyncAnonymousMiddleware<Unit, Unit, T1, T2>((_, executable, token) => {
      return middleware(t => executable.Execute(default, t), token);
    }));
  }

  /// <summary>
  /// Starts a parameterless async pipeline that returns value and can invoke downstream typed async action.
  /// </summary>
  /// <typeparam name="T1">Input type accepted by the downstream async action.</typeparam>
  /// <typeparam name="T2">Output type returned by the middleware.</typeparam>
  /// <param name="middleware">Async middleware function that can invoke downstream async action.</param>
  /// <returns>Builder with downstream output fixed to <see cref="Unit" />.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="middleware"/> is <see langword="null"/>.</exception>
  public static AsyncPipelineBuilder<Unit, T1, Unit, T2> Use<T1, T2>(AsyncFunc<AsyncAction<T1>, T2> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<Unit, T1, Unit, T2>(new AsyncAnonymousMiddleware<Unit, T1, Unit, T2>((_, executable, token) => {
      return middleware(async (i, t) => await executable.Execute(i, t), token);
    }));
  }

  /// <summary>
  /// Starts a parameterless async pipeline that returns value and can invoke downstream parameterless async action.
  /// </summary>
  /// <typeparam name="T">Output type returned by the middleware.</typeparam>
  /// <param name="middleware">Async middleware function that can invoke downstream parameterless async action.</param>
  /// <returns>Builder with downstream input/output fixed to <see cref="Unit" />.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="middleware"/> is <see langword="null"/>.</exception>
  public static AsyncPipelineBuilder<Unit, Unit, Unit, T> Use<T>(AsyncFunc<AsyncAction, T> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<Unit, Unit, Unit, T>(new AsyncAnonymousMiddleware<Unit, Unit, Unit, T>((_, executable, token) => {
      return middleware(async t => await executable.Execute(default, t), token);
    }));
  }

  /// <summary>
  /// Starts an async void pipeline from a step that receives downstream async function.
  /// </summary>
  /// <typeparam name="T1">Input type accepted by the middleware.</typeparam>
  /// <typeparam name="T2">Input type expected by the downstream async function.</typeparam>
  /// <typeparam name="T3">Output type returned by the downstream async function.</typeparam>
  /// <param name="middleware">Async middleware action that can invoke downstream function.</param>
  /// <returns>Builder for appending next middleware steps and terminal async executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="middleware"/> is <see langword="null"/>.</exception>
  public static AsyncPipelineBuilder<T1, T2, T3, Unit> Use<T1, T2, T3>(AsyncAction<T1, AsyncFunc<T2, T3>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<T1, T2, T3, Unit>(new AsyncAnonymousMiddleware<T1, T2, T3, Unit>(async (input, executable, token) => {
      await middleware(input, executable.Execute, token);
      return default;
    }));
  }

  /// <summary>
  /// Starts an async void pipeline from a step that receives downstream parameterless async function.
  /// </summary>
  /// <typeparam name="T1">Input type accepted by the middleware.</typeparam>
  /// <typeparam name="T2">Output type returned by the downstream parameterless async function.</typeparam>
  /// <param name="middleware">Async middleware action that can invoke downstream parameterless async function.</param>
  /// <returns>Builder with downstream input fixed to <see cref="Unit" />.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="middleware"/> is <see langword="null"/>.</exception>
  public static AsyncPipelineBuilder<T1, Unit, T2, Unit> Use<T1, T2>(AsyncAction<T1, AsyncFunc<T2>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<T1, Unit, T2, Unit>(new AsyncAnonymousMiddleware<T1, Unit, T2, Unit>(async (input, executable, token) => {
      await middleware(input, t => executable.Execute(default, t), token);
      return default;
    }));
  }

  /// <summary>
  /// Starts an async void pipeline from a step that forwards via typed async action.
  /// </summary>
  /// <typeparam name="T1">Input type accepted by the middleware.</typeparam>
  /// <typeparam name="T2">Input type accepted by the downstream async action.</typeparam>
  /// <param name="middleware">Async middleware action that calls downstream action.</param>
  /// <returns>Builder whose downstream output type is <see cref="Unit" />.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="middleware"/> is <see langword="null"/>.</exception>
  public static AsyncPipelineBuilder<T1, T2, Unit, Unit> Use<T1, T2>(AsyncAction<T1, AsyncAction<T2>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<T1, T2, Unit, Unit>(new AsyncAnonymousMiddleware<T1, T2, Unit, Unit>(async (input, executable, token) => {
      await middleware(input, async (i, t) => await executable.Execute(i, t), token);
      return default;
    }));
  }

  /// <summary>
  /// Starts an async void pipeline from a step that forwards via parameterless async action.
  /// </summary>
  /// <typeparam name="T">Input type accepted by the middleware.</typeparam>
  /// <param name="middleware">Async middleware action that calls downstream action.</param>
  /// <returns>Builder whose downstream input and output types are <see cref="Unit" />.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="middleware"/> is <see langword="null"/>.</exception>
  public static AsyncPipelineBuilder<T, Unit, Unit, Unit> Use<T>(AsyncAction<T, AsyncAction> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<T, Unit, Unit, Unit>(new AsyncAnonymousMiddleware<T, Unit, Unit, Unit>(async (input, executable, token) => {
      await middleware(input, async t => await executable.Execute(default, t), token);
      return default;
    }));
  }

  /// <summary>
  /// Starts a parameterless async void pipeline from a step that receives downstream typed async function.
  /// </summary>
  /// <typeparam name="T1">Input type expected by the downstream async function.</typeparam>
  /// <typeparam name="T2">Output type returned by the downstream async function.</typeparam>
  /// <param name="middleware">Async middleware action that can invoke downstream async function.</param>
  /// <returns>Builder for appending next middleware steps and terminal async executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="middleware"/> is <see langword="null"/>.</exception>
  public static AsyncPipelineBuilder<Unit, T1, T2, Unit> Use<T1, T2>(AsyncAction<AsyncFunc<T1, T2>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<Unit, T1, T2, Unit>(new AsyncAnonymousMiddleware<Unit, T1, T2, Unit>(async (_, executable, token) => {
      await middleware(executable.Execute, token);
      return default;
    }));
  }

  /// <summary>
  /// Starts a parameterless async void pipeline from a step that receives downstream parameterless async function.
  /// </summary>
  /// <typeparam name="T">Output type returned by the downstream parameterless async function.</typeparam>
  /// <param name="middleware">Async middleware action that can invoke downstream parameterless async function.</param>
  /// <returns>Builder with downstream input fixed to <see cref="Unit" />.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="middleware"/> is <see langword="null"/>.</exception>
  public static AsyncPipelineBuilder<Unit, Unit, T, Unit> Use<T>(AsyncAction<AsyncFunc<T>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<Unit, Unit, T, Unit>(new AsyncAnonymousMiddleware<Unit, Unit, T, Unit>(async (_, executable, token) => {
      await middleware(t => executable.Execute(default, t), token);
      return default;
    }));
  }

  /// <summary>
  /// Starts a parameterless async void pipeline from a step that receives downstream typed async action.
  /// </summary>
  /// <typeparam name="T">Input type accepted by the downstream async action.</typeparam>
  /// <param name="middleware">Async middleware action that can invoke downstream async action.</param>
  /// <returns>Builder with downstream output fixed to <see cref="Unit" />.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="middleware"/> is <see langword="null"/>.</exception>
  public static AsyncPipelineBuilder<Unit, T, Unit, Unit> Use<T>(AsyncAction<AsyncAction<T>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<Unit, T, Unit, Unit>(new AsyncAnonymousMiddleware<Unit, T, Unit, Unit>(async (_, executable, token) => {
      await middleware(async (i, t) => await executable.Execute(i, t), token);
      return default;
    }));
  }

  /// <summary>
  /// Starts a parameterless async pipeline from a step that receives next async action.
  /// </summary>
  /// <param name="middleware">Async middleware action that calls downstream action.</param>
  /// <returns>Builder whose all generic types are <see cref="Unit" />.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="middleware"/> is <see langword="null"/>.</exception>
  public static AsyncPipelineBuilder<Unit, Unit, Unit, Unit> Use(AsyncAction<AsyncAction> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<Unit, Unit, Unit, Unit>(new AsyncAnonymousMiddleware<Unit, Unit, Unit, Unit>(async (_, executable, token) => {
      await middleware(async t => await executable.Execute(default, t), token);
      return default;
    }));
  }

}