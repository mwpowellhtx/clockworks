using System;
using Kingdom.Unitworks.Dimensions;
using NUnit.Framework;

namespace Kingdom.Unitworks
{
    using L = Dimensions.Systems.SI.Length;

    public abstract class QuantityUnaryOperatorTestFixtureBase : QuantityTestFixtureBase
    {
        /// <summary>
        /// Gets a B Quantity.
        /// </summary>
        protected IQuantity B { get; private set; }

        /// <summary>
        /// Initializes the quantities.
        /// </summary>
        protected override void InitializeQuantities()
        {
            // For purposes of this test, does not matter what dimensions themselves are, per se.
            VerifyUnaryQuantity(A = new Quantity(Value, L.Meter));
            VerifyUnaryQuantity(B = new Quantity(Value));
        }

        /// <summary>
        /// Override to perform the specific ExpectedFunction.
        /// </summary>
        protected abstract Func<double, double> ExpectedFunction { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        protected double CalculateExpectedResult(double x)
        {
            return ExpectedFunction(x);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected bool TryInvokeOperator<T>(T value, out Quantity result)
        {
            result = value.InvokeOperator<T, Quantity>(Operator, value);
            return result != null;
        }

        private static void VerifyUnaryQuantity(IQuantity quantity)
        {
            Assert.That(quantity, Is.Not.Null);
            Assert.That(quantity.Dimensions, Is.Not.Null);
            Assert.That(quantity.Value, Is.EqualTo(Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="expectedValue"></param>
        public virtual void VerifyResults(Quantity result, Quantity value, double expectedValue)
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(value, Is.Not.Null);
            Assert.That(result, Is.Not.SameAs(value));

            Assert.That(value.Dimensions, Is.Not.Null);
            Assert.That(result.Dimensions, Is.Not.Null);
            Assert.That(result.Dimensions, Is.Not.SameAs(value.Dimensions));

            Assert.That(result.Dimensions.AreEquivalent(value.Dimensions));

            Console.WriteLine("Result: {0}", result);

            Assert.That(result.Value, Is.EqualTo(expectedValue));
        }
    }
}
