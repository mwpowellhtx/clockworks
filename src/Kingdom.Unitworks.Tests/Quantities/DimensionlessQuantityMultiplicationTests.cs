using System;
using NUnit.Framework;

namespace Kingdom.Unitworks
{
    public class DimensionlessQuantityMultiplicationTests
        : DimensionlessQuantityBinaryOperatorTestFixtureBase<Quantity, double>
    {
        protected override OperatorPart Operator
        {
            get { return OperatorPart.Multiply; }
        }

        protected override Func<double, double, double> ExpectedFunction
        {
            get { return (x, y) => x*y; }
        }

        protected override void InitializeQuantities()
        {
            VerifyDimensionlessQuantity(A = new Quantity(Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Multiply(Quantity,Quantity)"/>
        [Test]
        public void Verify_op_Multiply_Quantity_Quantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, a);
            VerifyResults(result, a, a, CalculateExpectedResult(Value, Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Multiply(Quantity,IQuantity)"/>
        [Test]
        public void Verify_op_Multiply_Quantity_IQuantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, A);
            VerifyResults(result, a, A, CalculateExpectedResult(Value, Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Multiply(IQuantity,Quantity)"/>
        [Test]
        public void Verify_op_Multiply_IQuantity_Quantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, A, a);
            VerifyResults(result, A, a, CalculateExpectedResult(Value, Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Multiply(Quantity,IQuantity)"/>
        [Test]
        public void Verify_op_Multiply_Assignment_Quantity_IQuantity()
        {
            var a = (Quantity) A;
            var result = CallOperatorAssignment(a, A, (x, y) => x *= y);
            VerifyResults(result, a, A, CalculateExpectedResult(Value, Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Multiply(Quantity,Quantity)"/>
        [Test]
        public void Verify_op_Multiply_Assignment_Quantity_Quantity()
        {
            var a = (Quantity) A;
            var result = CallOperatorAssignment(a, a, (x, y) => x *= y);
            VerifyResults(result, a, a, CalculateExpectedResult(Value, Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Multiply(Quantity,double)"/>
        [Test]
        public void Verify_op_Multiply_Quantity_double()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, Value);
            VerifyResults(result, a, Value, CalculateExpectedResult(Value, Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Multiply(double,Quantity)"/>
        [Test]
        public void Verify_op_Multiply_double_Quantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, Value, a);
            VerifyResults(result, a, Value, CalculateExpectedResult(Value, Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Multiply(Quantity,double)"/>
        [Test]
        public void Verify_op_Multiply_Assignment_Quantity_double()
        {
            var a = (Quantity) A;
            var result = CallOperatorAssignment(a, Value, (x, y) => x *= y);
            VerifyResults(result, a, Value, CalculateExpectedResult(Value, Value));
        }
    }
}
