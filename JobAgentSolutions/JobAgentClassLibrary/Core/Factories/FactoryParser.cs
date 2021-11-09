using System;
using System.Collections.Generic;

namespace JobAgentClassLibrary.Core.Factories
{
    public abstract class FactoryParser
    {
        [Obsolete("This method is out of date. Use ParseValue<T> instead.", error: true)]
        protected int ParseEntityValueToInt(object value)
        {
            if (value is int intVal)
            {
                return intVal;
            }

            throw new ArgumentException($"Value wasn't of type {typeof(int)}.", nameof(value));
        }

        [Obsolete("This method is out of date. Use ParseValue<T> instead.", error: true)]
        protected string ParseEntityValueToString(object value)
        {
            if (value is string strVal)
            {
                return strVal;
            }

            throw new ArgumentException($"Value wasn't of type {typeof(string)}.", nameof(value));
        }

        [Obsolete("This method is out of date. Use ParseValue<T> instead.", error: true)]
        protected List<T> ParseEntityValueToList<T>(object value)
        {
            if (value is List<T> list)
            {
                return list;
            }

            throw new ArgumentException($"Value wasn't of type {typeof(List<T>)}");
        }

        [Obsolete("This method is out of date. Use ParseValue<T> instead.", error: true)]
        protected DateTime ParseValueToDateTime(object value)
        {
            return DateTime.Now;
        }

        protected T ParseValue<T>(object value)
        {
            if (value is T parsedValue)
            {
                return parsedValue;
            }

            throw new ArgumentException($"Value wasn't of type {typeof(T)}");
        }
    }
}
