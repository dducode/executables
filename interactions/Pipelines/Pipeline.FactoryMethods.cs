using Interactions.Builders;
using Interactions.Core;

namespace Interactions.Pipelines;

public static partial class Pipeline<T1, T4> {

  public static PipelineBuilder<T1, T2, T3, T4> Use<T2, T3>(Func<T1, Handler<T2, T3>, T4> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new PipelineBuilder<T1, T2, T3, T4>(new AnonymousPipeline<T1, T2, T3, T4>(pipeline));
  }

  public static PipelineBuilder<T1, T2, T3, T4> Use<T2, T3>(Func<T1, Func<T2, T3>, T4> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new PipelineBuilder<T1, T2, T3, T4>(new AnonymousPipeline<T1, T2, T3, T4>((input, handler) => pipeline(input, handler.Handle)));
  }

  public static PipelineBuilder<T1, T2, Unit, T4> Use<T2>(Func<T1, Action<T2>, T4> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new PipelineBuilder<T1, T2, Unit, T4>(new AnonymousPipeline<T1, T2, Unit, T4>((input, handler) => {
      return pipeline(input, i => handler.Handle(i));
    }));
  }

  public static PipelineBuilder<T1, Unit, Unit, T4> Use(Func<T1, Action, T4> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new PipelineBuilder<T1, Unit, Unit, T4>(new AnonymousPipeline<T1, Unit, Unit, T4>((input, handler) => {
      return pipeline(input, () => handler.Handle(default));
    }));
  }

}

public static partial class Pipeline<T> {

  public static PipelineBuilder<T, T1, T2, Unit> Use<T1, T2>(Action<T, Handler<T1, T2>> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new PipelineBuilder<T, T1, T2, Unit>(new AnonymousPipeline<T, T1, T2>(pipeline));
  }

  public static PipelineBuilder<T, T1, T2, Unit> Use<T1, T2>(Action<T, Func<T1, T2>> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new PipelineBuilder<T, T1, T2, Unit>(new AnonymousPipeline<T, T1, T2>((input, handler) => pipeline(input, handler.Handle)));
  }

  public static PipelineBuilder<T, T1, Unit, Unit> Use<T1>(Action<T, Action<T1>> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new PipelineBuilder<T, T1, Unit, Unit>(new AnonymousPipeline<T, T1, Unit>((input, handler) => {
      pipeline(input, i => handler.Handle(i));
    }));
  }

  public static PipelineBuilder<T, Unit, Unit, Unit> Use(Action<T, Action> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new PipelineBuilder<T, Unit, Unit, Unit>(new AnonymousPipeline<T, Unit, Unit>((input, handler) => {
      pipeline(input, () => handler.Handle(default));
    }));
  }

}

public static partial class Pipeline {

  public static PipelineBuilder<Unit, Unit, Unit, Unit> Use(Action<Action> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new PipelineBuilder<Unit, Unit, Unit, Unit>(new AnonymousPipeline<Unit, Unit, Unit>((_, handler) => {
      pipeline(() => handler.Handle(default));
    }));
  }

}