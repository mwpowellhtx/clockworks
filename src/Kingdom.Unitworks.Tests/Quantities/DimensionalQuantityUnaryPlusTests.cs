using System;
using NUnit.Framework;

namespace Kingdom.Unitworks
{
    public class DimensionalQuantityUnaryPlusTests
        : QuantityUnaryOperatorTestFixtureBase
    {
        protected override OperatorPart Operator
        {
            get { return OperatorPart.UnaryPlus; }
        }

        protected override Func<double, double> ExpectedFunction
        {
            get { return x => +x; }
        }

        [Test]
        public void Verify_Dimensional_op_UnaryPlus_Quantity()
        {
            Quantity result;
            var a = (Quantity) A;
            Assert.That(TryInvokeOperator(a, out result));
            VerifyResults(result, a, CalculateExpectedResult(Value));
        }

        [Test]
        public void Verify_Dimensionless_op_UnaryPlus_Quantity()
        {
            Quantity result;
            var b = (Quantity) B;
            Assert.That(TryInvokeOperator(b, out result));
            VerifyResults(result, b, CalculateExpectedResult(Value));
        }
    }
}
