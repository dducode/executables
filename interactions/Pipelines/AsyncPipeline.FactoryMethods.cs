using Interactions.Builders;
using Interactions.Core;

namespace Interactions.Pipelines;

public static partial class Pipeline<T1, T4> {

  public static AsyncPipelineBuilder<T1, T2, T3, T4> Use<T2, T3>(AsyncFunc<T1, AsyncHandler<T2, T3>, T4> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new AsyncPipelineBuilder<T1, T2, T3, T4>(new AsyncAnonymousPipeline<T1, T2, T3, T4>(pipeline));
  }

  public static AsyncPipelineBuilder<T1, T2, T3, T4> Use<T2, T3>(AsyncFunc<T1, AsyncFunc<T2, T3>, T4> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new AsyncPipelineBuilder<T1, T2, T3, T4>(new AsyncAnonymousPipeline<T1, T2, T3, T4>((input, handler, token) =>
      pipeline(input, handler.Handle, token)
    ));
  }

  public static AsyncPipelineBuilder<T1, T2, Unit, T4> Use<T2>(AsyncFunc<T1, AsyncAction<T2>, T4> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new AsyncPipelineBuilder<T1, T2, Unit, T4>(new AsyncAnonymousPipeline<T1, T2, Unit, T4>((input, handler, token) => {
      return pipeline(input, async (i, t) => await handler.Handle(i, t), token);
    }));
  }

  public static AsyncPipelineBuilder<T1, Unit, Unit, T4> Use(AsyncFunc<T1, AsyncAction, T4> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new AsyncPipelineBuilder<T1, Unit, Unit, T4>(new AsyncAnonymousPipeline<T1, Unit, Unit, T4>((input, handler, token) => {
      return pipeline(input, async t => await handler.Handle(default, t), token);
    }));
  }

}

public static partial class Pipeline<T> {

  public static AsyncPipelineBuilder<T, T1, T2, Unit> Use<T1, T2>(AsyncAction<T, AsyncHandler<T1, T2>> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new AsyncPipelineBuilder<T, T1, T2, Unit>(new AsyncAnonymousPipeline<T, T1, T2>(pipeline));
  }

  public static AsyncPipelineBuilder<T, T1, T2, Unit> Use<T1, T2>(AsyncAction<T, AsyncFunc<T1, T2>> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new AsyncPipelineBuilder<T, T1, T2, Unit>(new AsyncAnonymousPipeline<T, T1, T2>((input, handler, token) =>
      pipeline(input, handler.Handle, token)
    ));
  }

  public static AsyncPipelineBuilder<T, T1, Unit, Unit> Use<T1>(AsyncAction<T, AsyncAction<T1>> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new AsyncPipelineBuilder<T, T1, Unit, Unit>(new AsyncAnonymousPipeline<T, T1, Unit>((input, handler, token) => {
      return pipeline(input, async (i, t) => await handler.Handle(i, t), token);
    }));
  }

  public static AsyncPipelineBuilder<T, Unit, Unit, Unit> Use(AsyncAction<T, AsyncAction> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new AsyncPipelineBuilder<T, Unit, Unit, Unit>(new AsyncAnonymousPipeline<T, Unit, Unit>((input, handler, token) => {
      return pipeline(input, async t => await handler.Handle(default, t), token);
    }));
  }

}

public static partial class Pipeline {

  public static AsyncPipelineBuilder<Unit, Unit, Unit, Unit> Use(AsyncAction<AsyncAction> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new AsyncPipelineBuilder<Unit, Unit, Unit, Unit>(new AsyncAnonymousPipeline<Unit, Unit, Unit>((_, handler, token) => {
      return pipeline(async t => await handler.Handle(default, t), token);
    }));
  }

}