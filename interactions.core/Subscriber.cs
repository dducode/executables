namespace Interactions.Core;

public interface ISubscriber<in T> : IExecutable<T, Unit>;