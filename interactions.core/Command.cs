namespace Interactions.Core;

public interface ICommand<in T> : IExecutable<T, bool>;

public class Command<T> : Handleable<T, Unit>, ICommand<T> {

  public virtual bool Execute(T input) {
    Handler<T, Unit> handler = Handler;
    if (handler == null)
      return false;

    handler.Execute(input);
    return true;
  }

}