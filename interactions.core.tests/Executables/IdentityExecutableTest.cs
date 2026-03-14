using Interactions.Core.Executables;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Executables;

[TestSubject(typeof(IdentityExecutable<>))]
public class IdentityExecutableTest {

  [Theory]
  [InlineData(10, 10)]
  [InlineData(1e-10f, 1e-10f)]
  [InlineData(true, true)]
  public void ReturnIdentityValue<T>(T expected, T actual) {
    IExecutor<T, T> executor = Executable.Identity<T>().GetExecutor();
    Assert.Equal(expected, executor.Execute(actual));
  }

}