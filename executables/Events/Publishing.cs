using Executables.Subscribers;

namespace Executables.Events;

public readonly struct Publishing<T>(T arg, List<ISubscriber<T>> subscribers) {

  public readonly T arg = arg;
  public readonly List<ISubscriber<T>> subscribers = subscribers;

}