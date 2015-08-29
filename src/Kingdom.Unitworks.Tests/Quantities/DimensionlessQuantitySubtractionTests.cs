using System;
using NUnit.Framework;

namespace Kingdom.Unitworks
{
    public class DimensionlessQuantitySubtractionTests
        : DimensionlessQuantityBinaryOperatorTestFixtureBase<Quantity, double>
    {
        protected override OperatorPart Operator
        {
            get { return OperatorPart.Subtraction; }
        }

        protected override Func<double, double, double> ExpectedFunction
        {
            get { return (x, y) => x - y; }
        }

        protected override void InitializeQuantities()
        {
            VerifyDimensionlessQuantity(A = new Quantity(Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Subtraction(Quantity,Quantity)"/>
        [Test]
        public void Verify_op_Subtraction_Quantity_Quantity()
        {
            /*TODO: while this is not a lot of lines of code pulling it together, I think with a little effort this could be contained/focused through a helper method
             * this breaks down a little bit because there are specialized verifications depending upon the use cases...
             * could even use the disposable context design pattern for this purpose ... ? */

            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, a);
            VerifyResults(result, a, a, CalculateExpectedResult(Value, Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Subtraction(Quantity,IQuantity)"/>
        [Test]
        public void Verify_op_Subtraction_Quantity_IQuantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, A);
            VerifyResults(result, a, A, CalculateExpectedResult(Value, Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Subtraction(IQuantity,Quantity)"/>
        [Test]
        public void Verify_op_Subtraction_IQuantity_Quantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, A, a);
            VerifyResults(result, A, a, CalculateExpectedResult(Value, Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Subtraction(Quantity,IQuantity)"/>
        [Test]
        public void Verify_op_Subtraction_Assignment_Quantity_IQuantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, A);
            VerifyResults(result, a, A, CalculateExpectedResult(Value, Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Subtraction(Quantity,Quantity)"/>
        [Test]
        public void Verify_op_Subtraction_Assignment_Quantity_Quantity()
        {
            /* This is interesting. It started out in life as the same reference,
             * but with Subtraction assignment, it ended up a different reference. */
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, a);
            VerifyResults(result, a, a, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_op_Subtraction_Quantity_double()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, Value);
            VerifyResults(result, a, Value, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_op_Subtraction_double_Quantity()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, Value, a);
            VerifyResults(result, a, Value, CalculateExpectedResult(Value, Value));
        }

        [Test]
        public void Verify_op_Subtraction_Assignment_Quantity_double()
        {
            var a = (Quantity) A;
            var result = CallOperatorAssignment(a, Value, (x, y) => x -= y);
            VerifyResults(result, a, Value, CalculateExpectedResult(Value, Value));
        }
    }
}
