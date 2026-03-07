using Interactions.Core.Executables;

namespace Interactions.Core.Subscribers;

public interface ISubscriber<in T> : IExecutable<T, Unit>;