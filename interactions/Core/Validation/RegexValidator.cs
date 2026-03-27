using System.Text.RegularExpressions;
using Interactions.Validation;

namespace Interactions.Core.Validation;

internal sealed class RegexValidator : Validator<string> {

  private readonly Regex _regex;

  internal RegexValidator(string pattern, RegexOptions options = RegexOptions.None) {
    _regex = new Regex(pattern, options);
    ErrorMessage = $"Value does not match pattern: {_regex}";
  }

  public override string ErrorMessage { get; }

  public override bool IsValid(string value) {
    return _regex.IsMatch(value);
  }

}