using PolicyLibrary.Validators.Abstractions;
using System;

namespace PolicyLibrary.Validators.Rules
{
    internal class IsNotNullRule : IPolicyRule
    {
        public void ValidateRule(object value, ref Validator validator)
        {
            if (value is null)
            {
                string msg = "Value is null.";
                validator.AddException(
                    new ArgumentException(msg, validator.Name));
            }
        }
    }
}
