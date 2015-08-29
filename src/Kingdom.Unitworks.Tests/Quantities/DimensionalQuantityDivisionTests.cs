using System;
using NUnit.Framework;

namespace Kingdom.Unitworks
{
    using L = Dimensions.Systems.SI.Length;
    using M = Dimensions.Systems.SI.Mass;

    //using SI_Length = Dimensions.Systems.SI.Length;
    //using SI_Mass = Dimensions.Systems.SI.Mass;
    //using US_Length = Dimensions.Systems.US.Length;
    //using US_Mass = Dimensions.Systems.US.Mass;

    public class DimensionalQuantityDivisionTests
        : DimensionalQuantityMultiplicativeTestFixtureBase
    {
        protected override OperatorPart Operator
        {
            get { return OperatorPart.Division; }
        }

        protected override Func<double, double, double> ExpectedFunction
        {
            get { return (x, y) => x/y; }
        }

        protected override void VerifyResults<TA, TB>(Quantity result, IQuantity a, IQuantity b, double expectedValue)
        {
            base.VerifyResults<TA, TB>(result, a, b.Inverse(), expectedValue);
        }

        public void VerifyResults(Quantity result, double a, IQuantity b, double expectedValue)
        {
            base.VerifyResults(result, b.Inverse(), a, expectedValue);
        }

        //TODO: TBD: want to be careful of units being in the correct dimensions...
        [Test]
        public void Verify_Homogenous_op_Divide_Quantity_Quantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, (Quantity) B);
            VerifyResults<Quantity, Quantity>(result, A, B, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_Homogenous_op_Divide_Quantity_IQuantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, B);
            VerifyResults<Quantity, IQuantity>(result, A, B, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_Homogenous_op_Divide_IQuantity_Quantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, A, (Quantity) B);
            VerifyResults<Quantity, IQuantity>(result, A, B, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_Homogenous_op_Divide_Assignment_Quantity_Quantity()
        {
            var a = (Quantity) A;
            var result = CallOperatorAssignment(a, (Quantity) B, (x, y) => x /= y);
            VerifyResults<Quantity, Quantity>(result, A, B, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_Homogenous_op_Divide_Assignment_Quantity_IQuantity()
        {
            var a = (Quantity) A;
            var result = CallOperatorAssignment(a, B, (x, y) => x /= y);
            VerifyResults<Quantity, IQuantity>(result, A, B, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_op_Divide_Quantity_double()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, Value);
            VerifyResults(result, a, Value, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_op_Divide_double_Quantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, Value, a);
            VerifyResults(result, Value, a, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_op_Divide_Assignment_Quantity_double()
        {
            var a = (Quantity) A;
            var result = CallOperatorAssignment(a, Value, (x, y) => x /= y);
            VerifyResults(result, a, Value, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_Heterogeneous_op_Divide_Quantity_Quantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, (Quantity) C);
            VerifyResults<Quantity, Quantity>(result, A, C, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_Heterogenous_op_Divide_Quantity_IQuantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, C);
            VerifyResults<Quantity, IQuantity>(result, A, C, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_Heterogenous_op_Divide_IQuantity_Quantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, A, (Quantity) C);
            VerifyResults<IQuantity, Quantity>(result, A, C, CalculateExpectedResult(Value, Value));
        }
    }
}
