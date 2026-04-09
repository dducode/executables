using Executables.Core.Executors;
using JetBrains.Annotations;

namespace Executables.Tests.Operators;

[TestSubject(typeof(ExceptionMapExecutor<,,>))]
public class ExceptionMapExecutorTest {

  [Fact]
  public void ConvertToCustomException() {
    IExecutor<string, int> executor = Executable
      .Create((string x) => int.Parse(x))
      .GetExecutor()
      .MapException((FormatException from) => new InvalidNumberException(from));

    Assert.Equal(0, executor.Execute("0"));
    var ex = Assert.Throws<InvalidNumberException>(() => executor.Execute("bad"));
    Assert.True(ex.InnerException is FormatException);
  }

}

file class InvalidNumberException(Exception inner) : Exception(null, inner);