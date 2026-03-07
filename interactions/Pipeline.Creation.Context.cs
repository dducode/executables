using Interactions.Context;
using Interactions.Core;
using Interactions.Core.Internal;
using Interactions.Pipelines;

namespace Interactions;

public static partial class Pipeline<T1, T4> {

  public static PipelineBuilder<T1, T2, T3, T4> Use<T2, T3>(Func<T1, ContextFunc<T2, T3>, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<T1, T2, T3, T4>(new AnonymousMiddleware<T1, T2, T3, T4>((input, executable) => middleware(input, executable.Execute)));
  }

  public static PipelineBuilder<T1, Unit, T, T4> Use<T>(Func<T1, ContextFunc<T>, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<T1, Unit, T, T4>(new AnonymousMiddleware<T1, Unit, T, T4>((input, executable) => {
      return middleware(input, init => executable.Execute(default, init));
    }));
  }

  public static PipelineBuilder<T1, T, Unit, T4> Use<T>(Func<T1, ContextAction<T>, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<T1, T, Unit, T4>(new AnonymousMiddleware<T1, T, Unit, T4>((input, executable) => {
      return middleware(input, (i, init) => executable.Execute(i, init));
    }));
  }

  public static PipelineBuilder<T1, Unit, Unit, T4> Use(Func<T1, ContextAction, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<T1, Unit, Unit, T4>(new AnonymousMiddleware<T1, Unit, Unit, T4>((input, executable) => {
      return middleware(input, init => executable.Execute(default, init));
    }));
  }

}

public static partial class Pipeline<T> {

  public static PipelineBuilder<Unit, T1, T2, T> Use<T1, T2>(Func<ContextFunc<T1, T2>, T> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<Unit, T1, T2, T>(new AnonymousMiddleware<Unit, T1, T2, T>((_, executable) => middleware(executable.Execute)));
  }

  public static PipelineBuilder<Unit, Unit, T1, T> Use<T1>(Func<ContextFunc<T1>, T> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<Unit, Unit, T1, T>(new AnonymousMiddleware<Unit, Unit, T1, T>((_, executable) => {
      return middleware(init => executable.Execute(default, init));
    }));
  }

  public static PipelineBuilder<Unit, T1, Unit, T> Use<T1>(Func<ContextAction<T1>, T> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<Unit, T1, Unit, T>(new AnonymousMiddleware<Unit, T1, Unit, T>((_, executable) => {
      return middleware((input, init) => executable.Execute(input, init));
    }));
  }

  public static PipelineBuilder<Unit, Unit, Unit, T> Use(Func<ContextAction, T> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<Unit, Unit, Unit, T>(new AnonymousMiddleware<Unit, Unit, Unit, T>((_, executable) => {
      return middleware(init => executable.Execute(default, init));
    }));
  }

  public static PipelineBuilder<T, T1, T2, Unit> Use<T1, T2>(Action<T, ContextFunc<T1, T2>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<T, T1, T2, Unit>(new AnonymousMiddleware<T, T1, T2, Unit>((input, executable) => {
      middleware(input, executable.Execute);
      return default;
    }));
  }

  public static PipelineBuilder<T, Unit, T1, Unit> Use<T1>(Action<T, ContextFunc<T1>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<T, Unit, T1, Unit>(new AnonymousMiddleware<T, Unit, T1, Unit>((input, executable) => {
      middleware(input, init => executable.Execute(default, init));
      return default;
    }));
  }

  public static PipelineBuilder<T, T1, Unit, Unit> Use<T1>(Action<T, ContextAction<T1>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<T, T1, Unit, Unit>(new AnonymousMiddleware<T, T1, Unit, Unit>((input, executable) => {
      middleware(input, (i, init) => executable.Execute(i, init));
      return default;
    }));
  }

  public static PipelineBuilder<T, Unit, Unit, Unit> Use(Action<T, ContextAction> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<T, Unit, Unit, Unit>(new AnonymousMiddleware<T, Unit, Unit, Unit>((input, executable) => {
      middleware(input, init => executable.Execute(default, init));
      return default;
    }));
  }

}

public static partial class Pipeline {

  public static PipelineBuilder<Unit, T1, T2, Unit> Use<T1, T2>(Action<ContextFunc<T1, T2>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<Unit, T1, T2, Unit>(new AnonymousMiddleware<Unit, T1, T2, Unit>((_, executable) => {
      middleware(executable.Execute);
      return default;
    }));
  }

  public static PipelineBuilder<Unit, Unit, T, Unit> Use<T>(Action<ContextFunc<T>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<Unit, Unit, T, Unit>(new AnonymousMiddleware<Unit, Unit, T, Unit>((_, executable) => {
      middleware(init => executable.Execute(default, init));
      return default;
    }));
  }

  public static PipelineBuilder<Unit, T, Unit, Unit> Use<T>(Action<ContextAction<T>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<Unit, T, Unit, Unit>(new AnonymousMiddleware<Unit, T, Unit, Unit>((_, executable) => {
      middleware((i, init) => executable.Execute(i, init));
      return default;
    }));
  }

  public static PipelineBuilder<Unit, Unit, Unit, Unit> Use(Action<ContextAction> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<Unit, Unit, Unit, Unit>(new AnonymousMiddleware<Unit, Unit, Unit, Unit>((_, executable) => {
      middleware((init) => executable.Execute(default, init));
      return default;
    }));
  }

}