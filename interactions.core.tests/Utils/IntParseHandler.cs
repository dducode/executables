namespace Interactions.Core.Tests.Utils;

internal sealed class IntParseHandler : Handler<string, int> {

  protected override int ExecuteCore(string input) {
    return int.Parse(input);
  }

}