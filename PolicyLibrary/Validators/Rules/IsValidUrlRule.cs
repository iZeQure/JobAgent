using PolicyLibrary.Validators.Abstractions;
using System;
using System.Net;

namespace PolicyLibrary.Validators.Rules
{
    internal class IsValidUrlRule : IPolicyRule
    {
        public void ValidateRule(object value, ref Validator validator)
        {
            if (value is string url)
            {
                // Is populated, if valid url.
                Uri validUrl = null;

                bool valid = Uri.TryCreate(url, UriKind.Absolute, out validUrl);

                if (!valid)
                {
                    string msg = $"{value} is not a valid url.";
                    validator.AddException(
                        new ArgumentException(msg, validator.Name));
                }
                
            }
        }
    }
}
