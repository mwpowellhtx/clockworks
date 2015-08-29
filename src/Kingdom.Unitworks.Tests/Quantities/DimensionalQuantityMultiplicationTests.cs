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

    public class DimensionalQuantityMultiplicationTests
        : DimensionalQuantityMultiplicativeTestFixtureBase
    {
        protected override OperatorPart Operator
        {
            get { return OperatorPart.Multiply; }
        }

        protected override Func<double, double, double> ExpectedFunction
        {
            get { return (x, y) => x*y; }
        }

        //TODO: TBD: want to be careful of units being in the correct dimensions...
        [Test]
        public void Verify_Homogenous_op_Multiply_Quantity_Quantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, (Quantity) A, (Quantity) B);
            VerifyResults(result, (Quantity) A, (Quantity) B, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_Homogenous_op_Multiply_Quantity_IQuantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, (Quantity) A, B);
            VerifyResults(result, (Quantity) A, B, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_Homogenous_op_Multiply_IQuantity_Quantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, A, (Quantity) B);
            VerifyResults(result, A, (Quantity) B, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_Homogenous_op_Multiply_Assignment_Quantity_Quantity()
        {
            var a = (Quantity) A;
            var result = CallOperatorAssignment((Quantity) A, (Quantity) B, (x, y) => x *= y);
            VerifyResults(result, (Quantity) A, (Quantity) B, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_Homogenous_op_Multiply_Assignment_Quantity_IQuantity()
        {
            var a = (Quantity) A;
            var result = CallOperatorAssignment((Quantity) A, B, (x, y) => x *= y);
            VerifyResults(result, (Quantity) A, B, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_op_Multiply_Quantity_double()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, (Quantity) A, Value);
            VerifyResults(result, (Quantity) A, Value, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_op_Multiply_double_Quantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, Value, (Quantity) A);
            VerifyResults(result, (Quantity) A, Value, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_op_Multiply_Assignment_Quantity_double()
        {
            var a = (Quantity) A;
            var result = CallOperatorAssignment((Quantity) A, Value, (x, y) => x *= y);
            VerifyResults(result, (Quantity) A, Value, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_Heterogeneous_op_Multiply_Quantity_Quantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, (Quantity) A, (Quantity) C);
            VerifyResults(result, (Quantity) A, (Quantity) C, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_Heterogenous_op_Multiply_Quantity_IQuantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, (Quantity) A, C);
            VerifyResults(result, (Quantity) A, C, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_Heterogenous_op_Multiply_IQuantity_Quantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, A, (Quantity) C);
            VerifyResults(result, A, (Quantity) C, CalculateExpectedResult(Value, Value));
        }
    }
}
