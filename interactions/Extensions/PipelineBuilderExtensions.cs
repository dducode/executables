using System.Diagnostics.Contracts;
using Interactions.Builders;
using Interactions.Core;
using Interactions.Pipelines;

namespace Interactions.Extensions;

/// <summary>
/// Convenience overloads for composing synchronous pipeline builders.
/// </summary>
/// <remarks>
/// These overloads allow defining middleware with <see cref="Func{T,TResult}" /> and <see cref="Action" />
/// delegates instead of constructing <see cref="Pipeline{T1,T2,T3,T4}" /> instances manually.
/// </remarks>
public static partial class PipelineBuilderExtensions {

  /// <summary>
  /// Appends middleware step expressed as function with next func.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Input type of downstream function.</typeparam>
  /// <typeparam name="T4">Output type of downstream function.</typeparam>
  /// <typeparam name="T5">Output type produced by current step.</typeparam>
  /// <typeparam name="T6">Output type of the final composed handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="pipeline">Middleware delegate with downstream function access.</param>
  /// <returns>Builder with updated downstream type pair <typeparamref name="T3" />/<typeparamref name="T4" />.</returns>
  public static PipelineBuilder<T1, T3, T4, T6> Use<T1, T2, T3, T4, T5, T6>(
    this PipelineBuilder<T1, T2, T5, T6> builder,
    Func<T2, Func<T3, T4>, T5> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AnonymousPipeline<T2, T3, T4, T5>((input, handler) => pipeline(input, handler.Handle)));
  }

  /// <summary>
  /// Appends middleware step expressed as function with next action.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Input type of downstream action.</typeparam>
  /// <typeparam name="T4">Output type produced by current step.</typeparam>
  /// <typeparam name="T5">Output type of the final composed handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="pipeline">Middleware delegate with downstream action access.</param>
  /// <returns>Builder with downstream output fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, T3, Unit, T5> Use<T1, T2, T3, T4, T5>(
    this PipelineBuilder<T1, T2, T4, T5> builder,
    Func<T2, Action<T3>, T4> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AnonymousPipeline<T2, T3, Unit, T4>((input, handler) => {
      return pipeline(input, i => handler.Handle(i));
    }));
  }

  /// <summary>
  /// Appends middleware step expressed as function with no args.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Output type produced by current step.</typeparam>
  /// <typeparam name="T4">Output type of the final composed handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="pipeline">Middleware delegate with parameterless downstream action access.</param>
  /// <returns>Builder with downstream input/output fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, Unit, Unit, T4> Use<T1, T2, T3, T4>(
    this PipelineBuilder<T1, T2, T3, T4> builder,
    Func<T2, Action, T3> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AnonymousPipeline<T2, Unit, Unit, T3>((input, handler) => {
      return pipeline(input, () => handler.Handle(default));
    }));
  }

  /// <summary>
  /// Appends middleware step expressed as action with next func.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Input type of downstream function.</typeparam>
  /// <typeparam name="T4">Output type of downstream function.</typeparam>
  /// <typeparam name="T5">Output type of the final composed handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="pipeline">Middleware action with downstream function access.</param>
  /// <returns>Builder with updated downstream type pair <typeparamref name="T3" />/<typeparamref name="T4" />.</returns>
  public static PipelineBuilder<T1, T3, T4, T5> Use<T1, T2, T3, T4, T5>(
    this PipelineBuilder<T1, T2, Unit, T5> builder,
    Action<T2, Func<T3, T4>> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AnonymousPipeline<T2, T3, T4>((input, handler) => pipeline(input, handler.Handle)));
  }

  /// <summary>
  /// Appends middleware step expressed as action with next action.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Input type of downstream action.</typeparam>
  /// <typeparam name="T4">Output type of the final composed handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="pipeline">Middleware action with downstream typed action access.</param>
  /// <returns>Builder with downstream output fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, T3, Unit, T4> Use<T1, T2, T3, T4>(
    this PipelineBuilder<T1, T2, Unit, T4> builder,
    Action<T2, Action<T3>> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AnonymousPipeline<T2, T3, Unit>((input, handler) => {
      pipeline(input, i => handler.Handle(i));
    }));
  }

  /// <summary>
  /// Appends middleware step expressed as action with no args.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Output type of the final composed handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="pipeline">Middleware action with parameterless downstream action access.</param>
  /// <returns>Builder with downstream input/output fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, Unit, Unit, T3> Use<T1, T2, T3>(
    this PipelineBuilder<T1, T2, Unit, T3> builder,
    Action<T2, Action> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AnonymousPipeline<T2, Unit, Unit>((input, handler) => {
      pipeline(input, () => handler.Handle(default));
    }));
  }

  /// <summary>
  /// Appends middleware step to a parameterless continuation chain.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed handler.</typeparam>
  /// <typeparam name="T2">Output type of the final composed handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="pipeline">Middleware action with parameterless downstream action access.</param>
  /// <returns>Builder with downstream input/output fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, Unit, Unit, T2> Use<T1, T2>(
    this PipelineBuilder<T1, Unit, Unit, T2> builder,
    Action<Action> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AnonymousPipeline<Unit, Unit, Unit>((_, handler) => {
      pipeline(() => handler.Handle(default));
    }));
  }

  /// <summary>
  /// Finalizes pipeline by converting function into terminal handler.
  /// </summary>
  /// <typeparam name="T1">Input type of the composed handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by terminal function.</typeparam>
  /// <typeparam name="T3">Output type returned by terminal function.</typeparam>
  /// <typeparam name="T4">Output type of the composed handler.</typeparam>
  /// <param name="builder">Builder being finalized.</param>
  /// <param name="action">Terminal function used as final handler.</param>
  /// <returns>Composed handler ready to be attached to handleable.</returns>
  [Pure]
  public static Handler<T1, T4> End<T1, T2, T3, T4>(this PipelineBuilder<T1, T2, T3, T4> builder, Func<T2, T3> action) {
    return builder.End(Handler.FromMethod(action));
  }

  /// <summary>
  /// Finalizes pipeline by converting action into terminal handler.
  /// </summary>
  /// <typeparam name="T1">Input type of the composed handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by terminal action.</typeparam>
  /// <typeparam name="T3">Output type of the composed handler.</typeparam>
  /// <param name="builder">Builder being finalized.</param>
  /// <param name="action">Terminal action used as final handler.</param>
  /// <returns>Composed handler ready to be attached to handleable.</returns>
  [Pure]
  public static Handler<T1, T3> End<T1, T2, T3>(this PipelineBuilder<T1, T2, Unit, T3> builder, Action<T2> action) {
    return builder.End(Handler.FromMethod(action));
  }

  /// <summary>
  /// Finalizes pipeline by converting parameterless action to handler.
  /// </summary>
  /// <typeparam name="T1">Input type of the composed handler.</typeparam>
  /// <typeparam name="T2">Output type of the composed handler.</typeparam>
  /// <param name="builder">Builder being finalized.</param>
  /// <param name="action">Terminal parameterless action used as final handler.</param>
  /// <returns>Composed handler ready to be attached to handleable.</returns>
  [Pure]
  public static Handler<T1, T2> End<T1, T2>(this PipelineBuilder<T1, Unit, Unit, T2> builder, Action action) {
    return builder.End(Handler.FromMethod(action));
  }

}