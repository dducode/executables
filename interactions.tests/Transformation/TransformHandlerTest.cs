using Interactions.Core.Handlers;
using Interactions.Core.Queries;
using Interactions.Handlers;
using Interactions.Transformation;
using Interactions.Transformation.Filtering;
using Interactions.Transformation.Parsing;
using JetBrains.Annotations;
using static Interactions.Validation.Validator;

namespace Interactions.Tests.Transformation;

[TestSubject(typeof(TransformHandler<,,,>))]
public class TransformHandlerTest {

  [Fact]
  public void ParseNumberTest() {
    var query = new Query<string, string>();
    using IDisposable handle = query.Handle(Handler.Create((int num) => num + num).Parse(Parser.Integer()));

    Assert.Equal("84", query.Execute("42"));
    Assert.Throws<FormatException>(() => query.Execute("not-a-number"));
    Assert.Throws<OverflowException>(() => query.Execute("1251328907421983752137032985702938"));
  }

  [Fact]
  public void FilterListTest() {
    var query = new Query<IEnumerable<string>, string>();
    using IDisposable handle = query.Handle(Handler
      .Identity<string>()
      .InputTransform(Transformer.First<string>())
      .InputFilter(Filter.Where(StringLength(MoreThan(2))))
    );

    Assert.Equal("input", query.Execute(new List<string> { string.Empty, "10", "input" }));
  }

}