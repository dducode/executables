using Interactions.Core;
using JetBrains.Annotations;

namespace Interactions.Tests.Transformation;

[TestSubject(typeof(IExecutable<,>))]
public class SplitConcatStringsTransformerTest {

  [Theory]
  [InlineData(new[] { "test", "test", "test" }, "test, test, test")]
  public void SplitTest(string[] expected, string input) {
    IExecutable<string, string[]> transformer = Transformer.Split(", ");
    Assert.Equal(expected, transformer.Execute(input));
  }

}