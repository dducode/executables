using Interactions.Core.Handlers;

namespace Interactions.Core.Tests.Utils;

internal static class TestHandler {

  internal static Handler<T, string> ToStringHandler<T>() {
    return new ToStringHandler<T>();
  }

  internal static Handler<string, int> IntParseHandler() {
    return new IntParseHandler();
  }

}