using Interactions.Builders;
using Interactions.Core;

namespace Interactions.Pipelines;

/// <summary>
/// Entry point for building synchronous pipelines that return a value.
/// </summary>
/// <typeparam name="T1">Input type of the final composed handler.</typeparam>
/// <typeparam name="T4">Output type of the final composed handler.</typeparam>
public static partial class Pipeline<T1, T4> {

  /// <summary>
  /// Starts a pipeline from a step that receives downstream call as <see cref="Func{T2,T3}" />.
  /// </summary>
  /// <typeparam name="T2">Input type expected by the downstream function.</typeparam>
  /// <typeparam name="T3">Output type returned by the downstream function.</typeparam>
  /// <param name="pipeline">Middleware function that can invoke downstream function.</param>
  /// <returns>Builder for appending next middleware steps and terminal handler.</returns>
  public static PipelineBuilder<T1, T2, T3, T4> Use<T2, T3>(Func<T1, Func<T2, T3>, T4> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new PipelineBuilder<T1, T2, T3, T4>(new AnonymousPipeline<T1, T2, T3, T4>((input, handler) => pipeline(input, handler.Handle)));
  }

  /// <summary>
  /// Starts a pipeline from a step that forwards to downstream via typed <see cref="Action{T}" />.
  /// </summary>
  /// <typeparam name="T2">Input type accepted by the downstream action.</typeparam>
  /// <param name="pipeline">Middleware function that calls downstream action.</param>
  /// <returns>Builder whose downstream output type is <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, T2, Unit, T4> Use<T2>(Func<T1, Action<T2>, T4> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new PipelineBuilder<T1, T2, Unit, T4>(new AnonymousPipeline<T1, T2, Unit, T4>((input, handler) => {
      return pipeline(input, i => handler.Handle(i));
    }));
  }

  /// <summary>
  /// Starts a pipeline from a step that forwards to downstream via parameterless <see cref="Action" />.
  /// </summary>
  /// <param name="pipeline">Middleware function that calls downstream action.</param>
  /// <returns>Builder whose downstream input and output types are <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, Unit, Unit, T4> Use(Func<T1, Action, T4> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new PipelineBuilder<T1, Unit, Unit, T4>(new AnonymousPipeline<T1, Unit, Unit, T4>((input, handler) => {
      return pipeline(input, () => handler.Handle(default));
    }));
  }

}

/// <summary>
/// Entry point for building synchronous pipelines that return <see cref="Unit" />.
/// </summary>
/// <typeparam name="T">Input type of the final composed handler.</typeparam>
public static partial class Pipeline<T> {

  /// <summary>
  /// Starts a void pipeline from a step that receives downstream function.
  /// </summary>
  /// <typeparam name="T1">Input type expected by the downstream function.</typeparam>
  /// <typeparam name="T2">Output type returned by the downstream function.</typeparam>
  /// <param name="pipeline">Middleware action that can invoke downstream function.</param>
  /// <returns>Builder for appending next middleware steps and terminal handler.</returns>
  public static PipelineBuilder<T, T1, T2, Unit> Use<T1, T2>(Action<T, Func<T1, T2>> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new PipelineBuilder<T, T1, T2, Unit>(new AnonymousPipeline<T, T1, T2>((input, handler) => pipeline(input, handler.Handle)));
  }

  /// <summary>
  /// Starts a void pipeline from a step that forwards via typed action.
  /// </summary>
  /// <typeparam name="T1">Input type accepted by the downstream action.</typeparam>
  /// <param name="pipeline">Middleware action that calls downstream action.</param>
  /// <returns>Builder whose downstream output type is <see cref="Unit" />.</returns>
  public static PipelineBuilder<T, T1, Unit, Unit> Use<T1>(Action<T, Action<T1>> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new PipelineBuilder<T, T1, Unit, Unit>(new AnonymousPipeline<T, T1, Unit>((input, handler) => {
      pipeline(input, i => handler.Handle(i));
    }));
  }

  /// <summary>
  /// Starts a void pipeline from a step that forwards via parameterless action.
  /// </summary>
  /// <param name="pipeline">Middleware action that calls downstream action.</param>
  /// <returns>Builder whose downstream input and output types are <see cref="Unit" />.</returns>
  public static PipelineBuilder<T, Unit, Unit, Unit> Use(Action<T, Action> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new PipelineBuilder<T, Unit, Unit, Unit>(new AnonymousPipeline<T, Unit, Unit>((input, handler) => {
      pipeline(input, () => handler.Handle(default));
    }));
  }

}

/// <summary>
/// Entry point for building synchronous pipelines without input and output values.
/// </summary>
public static partial class Pipeline {

  /// <summary>
  /// Starts a parameterless pipeline from a step that receives next action.
  /// </summary>
  /// <param name="pipeline">Middleware action that calls downstream action.</param>
  /// <returns>Builder whose all generic types are <see cref="Unit" />.</returns>
  public static PipelineBuilder<Unit, Unit, Unit, Unit> Use(Action<Action> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new PipelineBuilder<Unit, Unit, Unit, Unit>(new AnonymousPipeline<Unit, Unit, Unit>((_, handler) => {
      pipeline(() => handler.Handle(default));
    }));
  }

}