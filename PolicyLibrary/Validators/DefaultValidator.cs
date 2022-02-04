using PolicyLibrary.Validators.Abstractions;
using PolicyLibrary.Validators.Rules;
using System;
using System.Collections.Generic;

namespace PolicyLibrary.Validators
{
    public class DefaultValidator
    {
        private readonly List<IPolicyRule> _urlRules = new()
        {
            new IsNotNullRule(),
            new IsNotEmptyRule(),
            new IsValidUrlRule(),
            new CanRequestUrlRule()
        };

        public bool ValidateUrl(string url)
        {
            Validator v = new("UrlValidationPolicy", _urlRules);

            if (v.Validate(url))
            {
                return true;
            }
            else
            {
                foreach (Exception exception in v.GetExceptions)
                {
                    throw exception;
                }
                return false;
            }
        }

    }
}
