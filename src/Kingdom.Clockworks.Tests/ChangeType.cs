using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// Represents the type of change that is to occur.
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
    /// Change extension methods.
    /// </summary>
    internal static class ChangeExtensionMethods
    {
        /// <summary>
        /// Represents a dictionary of possible change Names.
        /// </summary>
        private static readonly IDictionary<ChangeType, string> Names;

        /// <summary>
        /// Static Constructor
        /// </summary>
        static ChangeExtensionMethods()
        {
            const string opText = "op_";

            const ChangeType increment = ChangeType.Increment;
            const ChangeType decrement = ChangeType.Decrement;
            const ChangeType addition = ChangeType.Addition;
            const ChangeType subtraction = ChangeType.Subtraction;
            const ChangeType op = ChangeType.Operator;

            Names = new Dictionary<ChangeType, string>
            {
                {increment, increment.ToString()},
                {decrement, decrement.ToString()},
                {op | increment, opText + increment},
                {op | decrement, opText + decrement},
                {op | addition, opText + addition},
                {op | subtraction, opText + subtraction},
            };
        }

        /// <summary>
        /// Returns the method name corresponding to the <paramref name="value"/>.
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
