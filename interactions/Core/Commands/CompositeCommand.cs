using Interactions.Handling;
using Interactions.Internal;

namespace Interactions.Core.Commands;

internal sealed class CompositeCommand<T>(Command<T> first, Command<T> second) : Command<T> {

  public override bool Execute(T input) {
    return first.Execute(input) && second.Execute(input);
  }

  public override IDisposable Handle(Handler<T, Unit> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return Handleable.MergedHandle(first, second, handler);
  }

}