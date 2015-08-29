using System;
using NUnit.Framework;

namespace Kingdom.Unitworks
{
    using IDEX = Dimensions.IncompatibleDimensionsException;
    using L = Dimensions.Systems.SI.Length;

    public class DimensionlessQuantityModulusTests
        : DimensionlessQuantityBinaryOperatorTestFixtureBase<Quantity, double>
    {
        protected override OperatorPart Operator
        {
            get { return OperatorPart.Modulus; }
        }

        protected override Func<double, double, double> ExpectedFunction
        {
            get { return (x, y) => x%y; }
        }

        private IQuantity B { get; set; }
        private IQuantity C { get; set; }
        private IQuantity D { get; set; }

        protected override void InitializeQuantities()
        {
            const double three = 3d;
            const double four = 4d;

            VerifyDimensionlessQuantity(A = new Quantity(Value));
            VerifyDimensionlessQuantity(B = new Quantity(three), three);
            VerifyDimensionlessQuantity(C = new Quantity(four), four);

            // Does not matter what dimension for purposes of these unit tests.
            D = new Quantity(0d, L.Meter);
        }

        [Test]
        public void Verify_op_Modulus_Quantity_A_double_B()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, B.Value);
            VerifyResults(result, a, B.Value, CalculateExpectedResult(Value, B.Value));
        }

        [Test]
        public void Verify_op_Modulus_Quantity_A_Quantity_B()
        {
            var a = (Quantity) A;
            var b = (Quantity) B;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, b);
            VerifyResults(result, a, b, CalculateExpectedResult(Value, B.Value));
        }

        [Test]
        public void Verify_op_Modulus_Quantity_A_IQuantity_B()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, B);
            VerifyResults(result, a, B, CalculateExpectedResult(Value, B.Value));
        }

        [Test]
        public void Verify_op_Modulus_Quantity_A_double_C()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, C.Value);
            VerifyResults(result, a, C.Value, CalculateExpectedResult(Value, C.Value));
        }

        [Test]
        public void Verify_op_Modulus_Quantity_A_Quantity_C()
        {
            var a = (Quantity) A;
            var c = (Quantity) C;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, c);
            VerifyResults(result, a, c, CalculateExpectedResult(Value, C.Value));
        }

        [Test]
        public void Verify_op_Modulus_Quantity_A_IQuantity_C()
        {
            var a = (Quantity) A;
            var result = a.InvokeOperator<Quantity, Quantity>(Operator, a, C);
            VerifyResults(result, a, C, CalculateExpectedResult(Value, C.Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Modulus(Quantity,double)"/>
        [Test]
        public void Verify_op_Modulus_Assignment_Quantity_A_double_B()
        {
            var a = (Quantity) A;
            var result = CallOperatorAssignment(a, B.Value, (x, y) => x %= y);
            VerifyResults(result, a, B.Value, CalculateExpectedResult(Value, B.Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Modulus(Quantity,Quantity)"/>
        [Test]
        public void Verify_op_Modulus_Assignment_Quantity_A_Quantity_B()
        {
            var a = (Quantity) A;
            var b = (Quantity) B;
            var result = CallOperatorAssignment(a, b, (x, y) => x %= y);
            VerifyResults(result, a, b, CalculateExpectedResult(Value, B.Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Modulus(Quantity,IQuantity)"/>
        [Test]
        public void Verify_op_Modulus_Assignment_Quantity_A_IQuantity_B()
        {
            var a = (Quantity) A;
            var result = CallOperatorAssignment(a, B, (x, y) => x %= y);
            VerifyResults(result, a, B, CalculateExpectedResult(Value, B.Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Modulus(Quantity,double)"/>
        [Test]
        public void Verify_op_Modulus_Assignment_Quantity_A_double_C()
        {
            var a = (Quantity) A;
            var result = CallOperatorAssignment(a, C.Value, (x, y) => x %= y);
            VerifyResults(result, a, C.Value, CalculateExpectedResult(Value, C.Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Modulus(Quantity,Quantity)"/>
        [Test]
        public void Verify_op_Modulus_Assignment_Quantity_A_Quantity_C()
        {
            var a = (Quantity) A;
            var c = (Quantity) C;
            var result = CallOperatorAssignment(a, c, (x, y) => x %= y);
            VerifyResults(result, a, c, CalculateExpectedResult(Value, C.Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Quantity.op_Modulus(Quantity,IQuantity)"/>
        [Test]
        public void Verify_op_Modulus_Assignment_Quantity_A_IQuantity_C()
        {
            var a = (Quantity) A;
            var result = CallOperatorAssignment(a, C, (x, y) => x %= y);
            VerifyResults(result, a, C, CalculateExpectedResult(Value, C.Value));
        }

        [Test]
        public void Verify_op_Modulus_Quantity_Dimensional_Divisor()
        {
            IQuantity result = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.That(result, Is.Null);

            Action action = () => result = ((Quantity) A)%(Quantity) D;

            this.Throws<IDEX>(action);
        }

        [Test]
        public void Verify_op_Modulus_IQuantity_Dimensional_Divisor()
        {
            IQuantity result = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.That(result, Is.Null);

            Action action = () => result = ((Quantity) A)%D;

            this.Throws<IDEX>(action);
        }
    }
}
