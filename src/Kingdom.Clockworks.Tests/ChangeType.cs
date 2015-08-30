using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum ChangeType : uint
    {
        None = 0,
        Increment = 1 << 1,
        Decrement = 1 << 2,
        Addition = 1 << 3,
        Subtraction = 1 << 4,
        Operator = 1 << 5
    }

    /// <summary>
    /// 
    /// </summary>
    internal static class ChangeExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly IDictionary<ChangeType, string> Names;

        /// <summary>
        /// Static Constructor
        /// </summary>
        static ChangeExtensionMethods()
        {
            const string op = "op_";

            Names = new Dictionary<ChangeType, string>
            {
                {ChangeType.Increment, ChangeType.Increment.ToString()},
                {ChangeType.Decrement, ChangeType.Decrement.ToString()},
                {ChangeType.Operator | ChangeType.Increment, op + ChangeType.Increment},
                {ChangeType.Operator | ChangeType.Decrement, op + ChangeType.Decrement},
                {ChangeType.Operator | ChangeType.Addition, op + ChangeType.Addition},
                {ChangeType.Operator | ChangeType.Subtraction, op + ChangeType.Subtraction},
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetMethodName(this ChangeType value)
        {
            Assert.That(Names.ContainsKey(value));
            return Names[value];
        }
    }
}
