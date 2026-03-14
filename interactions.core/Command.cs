namespace Interactions.Core;

public interface ICommand<in T> : IExecutable<T, bool> {

  bool Execute(T input);

}

public class Command<T> : Handleable<T, Unit>, ICommand<T> {

  public virtual bool Execute(T input) {
    Handler<T, Unit> handler = Handler;
    if (handler == null)
      return false;

    handler.Handle(input);
    return true;
  }

  public Executor GetExecutor() {
    return new Executor(this);
  }

  IExecutor<T, bool> IExecutable<T, bool>.GetExecutor() {
    return GetExecutor();
  }

  public readonly struct Executor(ICommand<T> command) : IExecutor<T, bool> {

    public bool Execute(T input) {
      return command.Execute(input);
    }

  }

}