using System;
using NUnit.Framework;

namespace Kingdom.Unitworks
{
    public abstract class QuantityBinaryOperatorTestFixtureBase<TResult, TExpected> : QuantityTestFixtureBase
    {
        /// <summary>
        /// Override to perform the specific ExpectedFunction.
        /// </summary>
        protected abstract Func<double, double, TExpected> ExpectedFunction { get; }

        /// <summary>
        /// Returns the calculated expected result given <paramref name="x"/> and <paramref name="y"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected TExpected CalculateExpectedResult(double x, double y)
        {
            Assert.That(ExpectedFunction, Is.Not.Null);
            Console.WriteLine("Expected result: {0} {1} {2}", x, Operator.GetCharacter(), y);
            return ExpectedFunction(x, y);
        }

        /// <summary>
        /// Calls the operator assignment version of the operator. This is identical
        /// to the underlying operator except for the assignment portion.
        /// </summary>
        /// <typeparam name="TA"></typeparam>
        /// <typeparam name="TB"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        protected TResult CallOperatorAssignment<TA, TB>(TA a, TB b, Func<TA, TB, TResult> operation)
        {
            Assert.That(operation, Is.Not.Null);

            Console.WriteLine("Performing op_{0} ( {1} ) Assignment: {2} {3} {4}",
                Operator, string.Join(", ", a.GetType(), b.GetType()),
                a, Operator.GetCharacter(true), b);

            return operation(a, b);
        }

        /// <summary>
        /// Tries to call the operator assignment version of the same operator.
        /// </summary>
        /// <typeparam name="TA"></typeparam>
        /// <typeparam name="TB"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="operation"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected bool TryCallOperatorAssignment<TA, TB>(TA a, TB b, Func<TA, TB, TResult> operation, out TResult result)
        {
            result = CallOperatorAssignment(a, b, operation);
            return result != null;
        }

        protected virtual void VerifyResults(Quantity result, IQuantity a, IQuantity b, double expectedValue)
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(a, Is.Not.Null);
            Assert.That(b, Is.Not.Null);

            Console.WriteLine("Result: {0}", result);

            Assert.That(result, Is.Not.SameAs(a));
            Assert.That(result, Is.Not.SameAs(b));

            Assert.That(result.Dimensions, Is.Not.Null);

            Assert.That(result.Dimensions, Is.Not.SameAs(a.Dimensions));
            Assert.That(result.Dimensions, Is.Not.SameAs(b.Dimensions));
        }

        protected virtual void VerifyResults(Quantity result, IQuantity a, double b, double expectedValue)
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(a, Is.Not.Null);

            Console.WriteLine("Result: {0}", result);

            Assert.That(result, Is.Not.SameAs(a));

            Assert.That(result.Dimensions, Is.Not.Null);
            Assert.That(a.Dimensions, Is.Not.Null);

            Assert.That(result.Dimensions, Is.Not.SameAs(a.Dimensions));
        }
    }
}
