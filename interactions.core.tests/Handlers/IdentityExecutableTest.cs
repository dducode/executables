using Interactions.Core.Executables;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handlers;

[TestSubject(typeof(IdentityExecutable<>))]
public class IdentityExecutableTest {

  [Theory]
  [InlineData(10, 10)]
  [InlineData(1e-10f, 1e-10f)]
  [InlineData(true, true)]
  public void ReturnIdentityValue<T>(T expected, T actual) {
    IExecutable<T, T> executable = Executable.Identity<T>();
    Assert.Equal(expected, executable.Execute(actual));
  }

}