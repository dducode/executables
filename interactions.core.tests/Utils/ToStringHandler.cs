using Interactions.Core.Handlers;

namespace Interactions.Core.Tests.Utils;

internal sealed class ToStringHandler<T> : Handler<T, string> {

  protected override string ExecuteCore(T input) {
    return input.ToString();
  }

}