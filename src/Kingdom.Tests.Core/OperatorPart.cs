using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Kingdom
{
    /// <summary>
    /// 
    /// </summary>
    public enum OperatorPart
    {
        /// <summary>
        /// 
        /// </summary>
        Increment,

        /// <summary>
        /// 
        /// </summary>
        Decrement,

        /// <summary>
        /// 
        /// </summary>
        Addition,

        /// <summary>
        /// 
        /// </summary>
        Subtraction,

        //TODO: C# just "handles" this part, translating "a += b" into "a = a + b" auto-magically.
        ///// <summary>
        ///// 
        ///// </summary>
        //Assignment,

        /// <summary>
        /// 
        /// </summary>
        Multiply,

        /// <summary>
        /// 
        /// </summary>
        Division,

        /// <summary>
        /// 
        /// </summary>
        GreaterThan,

        /// <summary>
        /// 
        /// </summary>
        LessThan,

        /// <summary>
        /// 
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// 
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        /// 
        /// </summary>
        Equality,

        /// <summary>
        /// 
        /// </summary>
        Inequality,
    }

    /// <summary>
    /// <see cref="OperatorPart"/> extension methods.
    /// </summary>
    public static class OperatorPartExtensionMethods
    {
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
    }
}
