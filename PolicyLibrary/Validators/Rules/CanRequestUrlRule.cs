using PolicyLibrary.Validators.Abstractions;
using System;
using System.Net;

namespace PolicyLibrary.Validators.Rules
{
    internal class CanRequestUrlRule : IPolicyRule
    {
        public void ValidateRule(object value, ref Validator validator)
        {
            if (value is string url)
            {
                try
                {
                    HttpWebRequest requestValidUrl = WebRequest.Create(url) as HttpWebRequest;

                    requestValidUrl.Method = "HEAD";

                    using HttpWebResponse response = requestValidUrl.GetResponse() as HttpWebResponse;

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        string msg = $"{value} is not a valid url.";
                        validator.AddException(
                            new ArgumentException(msg, validator.Name));
                    }
                }
                catch (Exception)
                {
                    string msg = $"{value} is not a valid url.";
                    validator.AddException(
                        new ArgumentException(msg, validator.Name));
                }
            }

        }
    }
}
