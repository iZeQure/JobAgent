using PolicyLibrary.Validators.Abstractions;
using System;

namespace PolicyLibrary.Validators.Rules
{
    internal class IsNotEmptyRule : IPolicyRule
    {
        public void ValidateRule(object value, ref Validator validator)
        {
            if (value is string valueString)
            {
                if (string.IsNullOrEmpty(valueString))
                {
                    string msg = $"Value is empty.";
                    validator.AddException(
                        new ArgumentException(msg, validator.Name));
                }
            }

        }
    }
}
