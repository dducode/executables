using Interactions.Builders;
using Interactions.Core;

namespace Interactions.Pipelines;

/// <summary>
/// Entry point for building asynchronous pipelines that return a value.
/// </summary>
/// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
/// <typeparam name="T4">Output type of the final composed async handler.</typeparam>
public static partial class Pipeline<T1, T4> {

  /// <summary>
  /// Starts an async pipeline from a step that receives downstream call as async function.
  /// </summary>
  /// <typeparam name="T2">Input type expected by the downstream async function.</typeparam>
  /// <typeparam name="T3">Output type returned by the downstream async function.</typeparam>
  /// <param name="pipeline">Async middleware function that can invoke downstream function.</param>
  /// <returns>Builder for appending next middleware steps and terminal async handler.</returns>
  public static AsyncPipelineBuilder<T1, T2, T3, T4> Use<T2, T3>(AsyncFunc<T1, AsyncFunc<T2, T3>, T4> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new AsyncPipelineBuilder<T1, T2, T3, T4>(new AsyncAnonymousPipeline<T1, T2, T3, T4>((input, handler, token) =>
      pipeline(input, handler.Handle, token)
    ));
  }

  /// <summary>
  /// Starts an async pipeline from a step that forwards via typed async action.
  /// </summary>
  /// <typeparam name="T2">Input type accepted by the downstream async action.</typeparam>
  /// <param name="pipeline">Async middleware function that calls downstream action.</param>
  /// <returns>Builder whose downstream output type is <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T1, T2, Unit, T4> Use<T2>(AsyncFunc<T1, AsyncAction<T2>, T4> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new AsyncPipelineBuilder<T1, T2, Unit, T4>(new AsyncAnonymousPipeline<T1, T2, Unit, T4>((input, handler, token) => {
      return pipeline(input, async (i, t) => await handler.Handle(i, t), token);
    }));
  }

  /// <summary>
  /// Starts an async pipeline from a step that forwards via parameterless async action.
  /// </summary>
  /// <param name="pipeline">Async middleware function that calls downstream action.</param>
  /// <returns>Builder whose downstream input and output types are <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T1, Unit, Unit, T4> Use(AsyncFunc<T1, AsyncAction, T4> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new AsyncPipelineBuilder<T1, Unit, Unit, T4>(new AsyncAnonymousPipeline<T1, Unit, Unit, T4>((input, handler, token) => {
      return pipeline(input, async t => await handler.Handle(default, t), token);
    }));
  }

}

/// <summary>
/// Entry point for building asynchronous pipelines that return <see cref="Unit" />.
/// </summary>
/// <typeparam name="T">Input type of the final composed async handler.</typeparam>
public static partial class Pipeline<T> {

  /// <summary>
  /// Starts an async void pipeline from a step that receives downstream async function.
  /// </summary>
  /// <typeparam name="T1">Input type expected by the downstream async function.</typeparam>
  /// <typeparam name="T2">Output type returned by the downstream async function.</typeparam>
  /// <param name="pipeline">Async middleware action that can invoke downstream function.</param>
  /// <returns>Builder for appending next middleware steps and terminal async handler.</returns>
  public static AsyncPipelineBuilder<T, T1, T2, Unit> Use<T1, T2>(AsyncAction<T, AsyncFunc<T1, T2>> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new AsyncPipelineBuilder<T, T1, T2, Unit>(new AsyncAnonymousPipeline<T, T1, T2>((input, handler, token) =>
      pipeline(input, handler.Handle, token)
    ));
  }

  /// <summary>
  /// Starts an async void pipeline from a step that forwards via typed async action.
  /// </summary>
  /// <typeparam name="T1">Input type accepted by the downstream async action.</typeparam>
  /// <param name="pipeline">Async middleware action that calls downstream action.</param>
  /// <returns>Builder whose downstream output type is <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T, T1, Unit, Unit> Use<T1>(AsyncAction<T, AsyncAction<T1>> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new AsyncPipelineBuilder<T, T1, Unit, Unit>(new AsyncAnonymousPipeline<T, T1, Unit>((input, handler, token) => {
      return pipeline(input, async (i, t) => await handler.Handle(i, t), token);
    }));
  }

  /// <summary>
  /// Starts an async void pipeline from a step that forwards via parameterless async action.
  /// </summary>
  /// <param name="pipeline">Async middleware action that calls downstream action.</param>
  /// <returns>Builder whose downstream input and output types are <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T, Unit, Unit, Unit> Use(AsyncAction<T, AsyncAction> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new AsyncPipelineBuilder<T, Unit, Unit, Unit>(new AsyncAnonymousPipeline<T, Unit, Unit>((input, handler, token) => {
      return pipeline(input, async t => await handler.Handle(default, t), token);
    }));
  }

}

/// <summary>
/// Entry point for building asynchronous pipelines without input and output values.
/// </summary>
public static partial class Pipeline {

  /// <summary>
  /// Starts a parameterless async pipeline from a step that receives next async action.
  /// </summary>
  /// <param name="pipeline">Async middleware action that calls downstream action.</param>
  /// <returns>Builder whose all generic types are <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<Unit, Unit, Unit, Unit> Use(AsyncAction<AsyncAction> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new AsyncPipelineBuilder<Unit, Unit, Unit, Unit>(new AsyncAnonymousPipeline<Unit, Unit, Unit>((_, handler, token) => {
      return pipeline(async t => await handler.Handle(default, t), token);
    }));
  }

}