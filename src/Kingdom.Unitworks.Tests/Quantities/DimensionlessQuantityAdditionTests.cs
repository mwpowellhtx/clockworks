using System;
using NUnit.Framework;

namespace Kingdom.Unitworks
{
    public class DimensionlessQuantityAdditionTests
        : DimensionlessQuantityBinaryOperatorTestFixtureBase<Quantity, double>
    {
        protected override OperatorPart Operator
        {
            get { return OperatorPart.Addition; }
        }

        protected override Func<double, double, double> ExpectedFunction
        {
            get { return (x, y) => x + y; }
        }

        protected override void InitializeQuantities()
        {
            VerifyDimensionlessQuantity(A = new Quantity(Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Addition(Quantity,Quantity)"/>
        [Test]
        public void Verify_op_Addition_Quantity_Quantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, a);
            VerifyResults(result, a, a, CalculateExpectedResult(Value, Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Addition(Quantity,IQuantity)"/>
        [Test]
        public void Verify_op_Addition_Quantity_IQuantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, A);
            VerifyResults(result, a, A, CalculateExpectedResult(Value, Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Addition(IQuantity,Quantity)"/>
        [Test]
        public void Verify_op_Addition_IQuantity_Quantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, A, a);
            VerifyResults(result, A, a, CalculateExpectedResult(Value, Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Addition(Quantity,IQuantity)"/>
        [Test]
        public void Verify_op_Addition_Assignment_Quantity_IQuantity()
        {
            var a = (Quantity) A;
            var result = CallOperatorAssignment(a, A, (x, y) => x += y);
            VerifyResults(result, a, A, CalculateExpectedResult(Value, Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Addition(Quantity,Quantity)"/>
        [Test]
        public void Verify_op_Addition_Assignment_Quantity_Quantity()
        {
            var a = (Quantity) A;
            var result = CallOperatorAssignment(a, a, (x, y) => x += y);
            VerifyResults(result, A, A, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_op_Addition_Quantity_double()
        {
            var a = (Quantity) A;
            var result = CallOperatorAssignment(a, Value, (x, y) => x += y);
            VerifyResults(result, A, Value, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_op_Addition_double_Quantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, Value, a);
            VerifyResults(result, A, Value, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_op_Addition_Assignment_Quantity_double()
        {
            var a = (Quantity) A;
            var result = CallOperatorAssignment(a, Value, (x, y) => x += y);
            VerifyResults(result, a, Value, CalculateExpectedResult(Value, Value));
        }
    }
}
