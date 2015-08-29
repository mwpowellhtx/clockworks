using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Kingdom
{
    /// <summary>
    /// 
    /// </summary>
    public enum OperatorPart : ulong
    {
        /// <summary>
        /// 
        /// </summary>
        Increment = 1 << 1,

        /// <summary>
        /// 
        /// </summary>
        Decrement = 1 << 2,

        /// <summary>
        /// 
        /// </summary>
        Addition = 1 << 3,

        /// <summary>
        /// 
        /// </summary>
        Subtraction = 1 << 4,

        /// <summary>
        /// 
        /// </summary>
        UnaryPlus = 1 << 5,

        /// <summary>
        /// 
        /// </summary>
        UnaryNegation = 1 << 6,

        //TODO: C# just "handles" this part, translating "a += b" into "a = a + b" auto-magically.
        ///// <summary>
        ///// 
        ///// </summary>
        //Assignment=1<<7,

        /// <summary>
        /// 
        /// </summary>
        Multiply = 1 << 8,

        /// <summary>
        /// 
        /// </summary>
        Division = 1 << 9,

        /// <summary>
        /// 
        /// </summary>
        Modulus = 1 << 10,

        /// <summary>
        /// 
        /// </summary>
        GreaterThan = 1 << 12,

        /// <summary>
        /// 
        /// </summary>
        LessThan = 1 << 13,

        /// <summary>
        /// 
        /// </summary>
        GreaterThanOrEqual = 1 << 14,

        /// <summary>
        /// 
        /// </summary>
        LessThanOrEqual = 1 << 15,

        /// <summary>
        /// 
        /// </summary>
        Equality = 1 << 16,

        /// <summary>
        /// 
        /// </summary>
        Inequality = 1 << 17,
    }

    /// <summary>
    /// <see cref="OperatorPart"/> extension methods.
    /// </summary>
    public static class OperatorPartExtensionMethods
    {
        /// <summary>
        /// Returns the string representation of the given <see cref="OperatorPart"/>.
        /// </summary>
        /// <param name="op"></param>
        /// <param name="assignment"></param>
        /// <returns></returns>
        public static string GetCharacter(this OperatorPart op, bool assignment = false)
        {
            var text = string.Empty;

            switch (op)
            {
                case OperatorPart.Modulus:
                    text = "%";
                    break;
                case OperatorPart.Addition:
                case OperatorPart.UnaryPlus:
                    text = "+";
                    break;
                case OperatorPart.Subtraction:
                case OperatorPart.UnaryNegation:
                    text = "-";
                    break;
                case OperatorPart.Multiply:
                    text = "*";
                    break;
                case OperatorPart.Division:
                    text = "/";
                    break;
                case OperatorPart.Equality:
                    text = "==";
                    break;
                case OperatorPart.Inequality:
                    text = "!=";
                    break;
                case OperatorPart.LessThan:
                    text = "<";
                    break;
                case OperatorPart.GreaterThan:
                    text = ">";
                    break;
                case OperatorPart.LessThanOrEqual:
                    text = "<=";
                    break;
                case OperatorPart.GreaterThanOrEqual:
                    text = ">=";
                    break;
            }

            Assert.That(text, Has.Length.GreaterThan(0), "Operator {0} is unsupported at ths time.", op);

            return text + (assignment ? "=" : string.Empty);
        }

        /// <summary>
        /// Returns the member name from the <paramref name="parts"/>.
        /// </summary>
        /// <param name="parts"></param>
        /// <returns></returns>
        public static string GetMemberName(this IEnumerable<OperatorPart> parts)
        {
            Assert.That(parts, Is.Not.Null);
            // ReSharper disable PossibleMultipleEnumeration
            {
                CollectionAssert.IsNotEmpty(parts);
                return parts.Aggregate(@"op_", (g, x) => g + x.ToString());
            }
        }

        public static string GetMemberName(this OperatorPart op)
        {
            return string.Format("op_{0}", op);
        }
    }
}
