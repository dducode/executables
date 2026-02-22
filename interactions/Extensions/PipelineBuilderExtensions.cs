using System.Diagnostics.Contracts;
using Interactions.Builders;
using Interactions.Core;
using Interactions.Pipelines;

namespace Interactions.Extensions;

public static partial class PipelineBuilderExtensions {

  public static PipelineBuilder<T1, T3, T4, T6> Use<T1, T2, T3, T4, T5, T6>(
    this PipelineBuilder<T1, T2, T5, T6> builder,
    Func<T2, Func<T3, T4>, T5> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AnonymousPipeline<T2, T3, T4, T5>((input, handler) => pipeline(input, handler.Handle)));
  }

  public static PipelineBuilder<T1, T3, Unit, T5> Use<T1, T2, T3, T4, T5>(
    this PipelineBuilder<T1, T2, T4, T5> builder,
    Func<T2, Action<T3>, T4> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AnonymousPipeline<T2, T3, Unit, T4>((input, handler) => {
      return pipeline(input, i => handler.Handle(i));
    }));
  }

  public static PipelineBuilder<T1, Unit, Unit, T4> Use<T1, T2, T3, T4>(
    this PipelineBuilder<T1, T2, T3, T4> builder,
    Func<T2, Action, T3> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AnonymousPipeline<T2, Unit, Unit, T3>((input, handler) => {
      return pipeline(input, () => handler.Handle(default));
    }));
  }

  public static PipelineBuilder<T1, T3, T4, T5> Use<T1, T2, T3, T4, T5>(
    this PipelineBuilder<T1, T2, Unit, T5> builder,
    Action<T2, Func<T3, T4>> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AnonymousPipeline<T2, T3, T4>((input, handler) => pipeline(input, handler.Handle)));
  }

  public static PipelineBuilder<T1, T3, Unit, T4> Use<T1, T2, T3, T4>(
    this PipelineBuilder<T1, T2, Unit, T4> builder,
    Action<T2, Action<T3>> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AnonymousPipeline<T2, T3, Unit>((input, handler) => {
      pipeline(input, i => handler.Handle(i));
    }));
  }

  public static PipelineBuilder<T1, Unit, Unit, T3> Use<T1, T2, T3>(
    this PipelineBuilder<T1, T2, Unit, T3> builder,
    Action<T2, Action> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AnonymousPipeline<T2, Unit, Unit>((input, handler) => {
      pipeline(input, () => handler.Handle(default));
    }));
  }

  public static PipelineBuilder<T1, Unit, Unit, T2> Use<T1, T2>(
    this PipelineBuilder<T1, Unit, Unit, T2> builder,
    Action<Action> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AnonymousPipeline<Unit, Unit, Unit>((_, handler) => {
      pipeline(() => handler.Handle(default));
    }));
  }

  [Pure]
  public static Handler<T1, T4> End<T1, T2, T3, T4>(this PipelineBuilder<T1, T2, T3, T4> builder, Func<T2, T3> action) {
    return builder.End(Handler.FromMethod(action));
  }

  [Pure]
  public static Handler<T1, T3> End<T1, T2, T3>(this PipelineBuilder<T1, T2, Unit, T3> builder, Action<T2> action) {
    return builder.End(Handler.FromMethod(action));
  }

  [Pure]
  public static Handler<T1, T2> End<T1, T2>(this PipelineBuilder<T1, Unit, Unit, T2> builder, Action action) {
    return builder.End(Handler.FromMethod(action));
  }

}