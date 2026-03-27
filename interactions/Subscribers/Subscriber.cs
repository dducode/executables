namespace Interactions.Subscribers;

public interface ISubscriber<in T> : IExecutable<T, Unit> {

  void Receive(T input);

}