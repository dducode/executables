using Interactions.Core;
using Interactions.Executables;
using Interactions.Maps;
using Interactions.Transformation.Filtering;
using Interactions.Transformation.Parsing;
using JetBrains.Annotations;
using static Interactions.Validator;

namespace Interactions.Tests.Transformation;

[TestSubject(typeof(ExecutableMap<,,,>))]
public class ExecutableMapTest {

  [Fact]
  public void ParseNumberTest() {
    IExecutable<string, string> executable = Executable.Create((int num) => num + num).Parse(Parser.Integer());

    Assert.Equal("84", executable.Execute("42"));
    Assert.Throws<FormatException>(() => executable.Execute("not-a-number"));
    Assert.Throws<OverflowException>(() => executable.Execute("1251328907421983752137032985702938"));
  }

  [Fact]
  public void FilterListTest() {
    IExecutable<IEnumerable<string>, string> filter = Executable
      .Identity<string>()
      .InMap(Transformer.First<string>())
      .InFilter(Filter.Where(StringLength(MoreThan(2))));

    Assert.Equal("input", filter.Execute(new List<string> { string.Empty, "10", "input" }));
  }

}