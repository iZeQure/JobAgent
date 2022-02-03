using System;
using System.Collections.Generic;
using System.Linq;

namespace PolicyLibrary.Validators.Abstractions
{
    public class Validator : IRule
    {
        private readonly IReadOnlyCollection<IPolicyRule> _rules;
        private readonly List<Exception> _exceptions = new();
        private readonly string _name;

        public Validator(string name, List<IPolicyRule> rules)
        {
            _rules = rules;
            _name = name;
        }

        public List<Exception> GetExceptions
        {
            get
            {
                if (_exceptions is null)
                {
                    return null;
                }

                return _exceptions;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public bool Validate(object value)
        {
            if (_exceptions is null)
            {
                return false;
            }

            // Set to be used as a ref.
            Validator v = this;

            foreach (IPolicyRule rule in _rules)
            {
                rule.ValidateRule(value, ref v);
            }

            if (!_exceptions.Any())
            {
                return true;
            }

            return false;
        }

        internal void AddException(Exception ex)
        {
            _exceptions.Add(ex);
        }
    }
}
