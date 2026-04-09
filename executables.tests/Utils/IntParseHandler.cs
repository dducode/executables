using Executables.Handling;

namespace Executables.Tests.Utils;

internal sealed class IntParseHandler : Handler<string, int> {

  protected override int HandleCore(string input) {
    return int.Parse(input);
  }

}