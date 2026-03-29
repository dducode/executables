using Executables.Handling;

namespace Executables.Tests.Utils;

internal sealed class ToStringHandler<T> : Handler<T, string> {

  protected override string HandleCore(T input) {
    return input.ToString();
  }

}