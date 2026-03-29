using Executables.Handling;

namespace Executables.Tests.Utils;

internal static class TestHandler {

  internal static Handler<T, string> ToStringHandler<T>() {
    return new ToStringHandler<T>();
  }

  internal static Handler<string, int> IntParseHandler() {
    return new IntParseHandler();
  }

}