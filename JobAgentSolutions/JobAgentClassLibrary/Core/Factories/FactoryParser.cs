using System;

namespace JobAgentClassLibrary.Core.Factories
{
    public abstract class FactoryParser
    {
        protected int ParseEntityValueToInt(object value)
        {
            if (value is int intVal)
            {
                return intVal;
            }

            throw new ArgumentException("Value wasn't of type string.", nameof(value));
        }

        protected string ParseEntityValueToString(object value)
        {
            if (value is string strVal)
            {
                return strVal;
            }

            throw new ArgumentException("Value wasn't of type string.", nameof(value));
        }
    }
}
