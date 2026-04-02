using Executables.Core.Operators;
using JetBrains.Annotations;

namespace Executables.Tests.Operators;

[TestSubject(typeof(ExceptionMap<,,>))]
public class ExceptionMapTest {

  [Fact]
  public void ConvertToCustomException() {
    IQuery<string, int> query = Executable
      .Create((string x) => int.Parse(x))
      .MapException((FormatException from) => new InvalidNumberException(from))
      .AsQuery();

    Assert.Equal(0, query.Send("0"));
    var ex = Assert.Throws<InvalidNumberException>(() => query.Send("bad"));
    Assert.True(ex.InnerException is FormatException);
  }

}

file class InvalidNumberException(Exception inner) : Exception(null, inner);