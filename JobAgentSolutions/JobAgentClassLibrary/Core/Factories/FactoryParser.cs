using System;
using System.Collections.Generic;

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

            throw new ArgumentException($"Value wasn't of type {typeof(int)}.", nameof(value));
        }

        protected string ParseEntityValueToString(object value)
        {
            if (value is string strVal)
            {
                return strVal;
            }

            throw new ArgumentException($"Value wasn't of type {typeof(string)}.", nameof(value));
        }

        protected List<T> ParseEntityValueToList<T>(object value)
        {
            if (value is List<T> list)
            {
                return list;
            }

            throw new ArgumentException($"Value wasn't of type {typeof(List<T>)}");
        }
    }
}
