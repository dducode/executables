using System.Diagnostics.Contracts;
using Interactions.Builders;
using Interactions.Core;
using Interactions.Pipelines;

namespace Interactions.Extensions;

public static partial class PipelineBuilderExtensions {

  public static AsyncPipelineBuilder<T1, T3, T4, T6> Use<T1, T2, T3, T4, T5, T6>(
    this AsyncPipelineBuilder<T1, T2, T5, T6> builder,
    AsyncFunc<T2, AsyncFunc<T3, T4>, T5> pipeline) {
    return builder.Use(new AsyncAnonymousPipeline<T2, T3, T4, T5>((input, handler, token) => pipeline(input, handler.Handle, token)));
  }

  public static AsyncPipelineBuilder<T1, T3, Unit, T5> Use<T1, T2, T3, T4, T5>(
    this AsyncPipelineBuilder<T1, T2, T4, T5> builder,
    AsyncFunc<T2, AsyncAction<T3>, T4> pipeline) {
    return builder.Use(new AsyncAnonymousPipeline<T2, T3, Unit, T4>((input, handler, token) => {
      return pipeline(input, async (i, t) => await handler.Handle(i, t), token);
    }));
  }

  public static AsyncPipelineBuilder<T1, Unit, Unit, T4> Use<T1, T2, T3, T4>(
    this AsyncPipelineBuilder<T1, T2, T3, T4> builder,
    AsyncFunc<T2, AsyncAction, T3> pipeline) {
    return builder.Use(new AsyncAnonymousPipeline<T2, Unit, Unit, T3>((input, handler, token) => {
      return pipeline(input, async t => await handler.Handle(default, t), token);
    }));
  }

  public static AsyncPipelineBuilder<T1, T3, T4, T5> Use<T1, T2, T3, T4, T5>(
    this AsyncPipelineBuilder<T1, T2, Unit, T5> builder,
    AsyncAction<T2, AsyncFunc<T3, T4>> pipeline) {
    return builder.Use(new AsyncAnonymousPipeline<T2, T3, T4>((input, handler, token) => pipeline(input, handler.Handle, token)));
  }

  public static AsyncPipelineBuilder<T1, T3, Unit, T4> Use<T1, T2, T3, T4>(
    this AsyncPipelineBuilder<T1, T2, Unit, T4> builder,
    AsyncAction<T2, AsyncAction<T3>> pipeline) {
    return builder.Use(new AsyncAnonymousPipeline<T2, T3, Unit>((input, handler, token) => {
      return pipeline(input, async (i, t) => await handler.Handle(i, t), token);
    }));
  }

  public static AsyncPipelineBuilder<T1, Unit, Unit, T3> Use<T1, T2, T3>(
    this AsyncPipelineBuilder<T1, T2, Unit, T3> builder,
    AsyncAction<T2, AsyncAction> pipeline) {
    return builder.Use(new AsyncAnonymousPipeline<T2, Unit, Unit>((input, handler, token) => {
      return pipeline(input, async t => await handler.Handle(default, t), token);
    }));
  }

  public static AsyncPipelineBuilder<T1, Unit, Unit, T2> Use<T1, T2>(
    this AsyncPipelineBuilder<T1, Unit, Unit, T2> builder,
    AsyncAction<AsyncAction> pipeline) {
    return builder.Use(new AsyncAnonymousPipeline<Unit, Unit, Unit>((_, handler, token) => {
      return pipeline(async t => await handler.Handle(default, t), token);
    }));
  }

  [Pure]
  public static AsyncHandler<T1, T4> End<T1, T2, T3, T4>(this AsyncPipelineBuilder<T1, T2, T3, T4> builder, AsyncFunc<T2, T3> action) {
    return builder.End(Handler.FromMethod(action));
  }

  [Pure]
  public static AsyncHandler<T1, T3> End<T1, T2, T3>(this AsyncPipelineBuilder<T1, T2, Unit, T3> builder, AsyncAction<T2> action) {
    return builder.End(Handler.FromMethod(action));
  }

  [Pure]
  public static AsyncHandler<T1, T2> End<T1, T2>(this AsyncPipelineBuilder<T1, Unit, Unit, T2> builder, AsyncAction action) {
    return builder.End(Handler.FromMethod(action));
  }

}