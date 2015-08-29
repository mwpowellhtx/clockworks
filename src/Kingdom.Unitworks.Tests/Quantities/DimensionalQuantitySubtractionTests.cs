using System;
using NUnit.Framework;

namespace Kingdom.Unitworks
{
    using IDEX = Dimensions.IncompatibleDimensionsException;

    public class DimensionalQuantitySubtractionTests
        : DimensionalQuantityAdditiveTestFixtureBase
    {
        protected override OperatorPart Operator
        {
            get { return OperatorPart.Subtraction; }
        }

        protected override Func<double, double, double> ExpectedFunction
        {
            get { return (x, y) => x - y; }
        }

        [Test]
        public void Verify_Homogenous_op_Subtraction_Quantity_Quantity()
        {
            VerifyHomogeneousDimensions(A, A);

            this.DoesNotThrow<IDEX>(() =>
            {
                Quantity result;
                var a = (Quantity) A;
                Assert.That(a.TryInvokeOperator(Operator, out result, a, a));
                VerifyResults(result, a, a, CalculateExpectedResult(Value, Value));
            });
        }

        [Test]
        public void Verify_Homogenous_op_Subtraction_Quantity_IQuantity()
        {
            VerifyHomogeneousDimensions(A, A);

            this.DoesNotThrow<IDEX>(() =>
            {
                Quantity result;
                var a = (Quantity) A;
                Assert.That(a.TryInvokeOperator(Operator, out result, a, A));
                VerifyResults(result, a, A, CalculateExpectedResult(Value, Value));
            });
        }

        [Test]
        public void Verify_Homogenous_op_Subtraction_IQuantity_Quantity()
        {
            VerifyHomogeneousDimensions(A, A);

            this.DoesNotThrow<IDEX>(() =>
            {
                Quantity result;
                var a = (Quantity) A;
                Assert.That(a.TryInvokeOperator(Operator, out result, A, a));
                VerifyResults(result, A, a, CalculateExpectedResult(Value, Value));
            });
        }

        [Test]
        public void Verify_Homogenous_op_Subtraction_Assignment_Quantity_Quantity()
        {
            VerifyHomogeneousDimensions(A, A);

            this.DoesNotThrow<IDEX>(() =>
            {
                Quantity result;
                var a = (Quantity) A;
                Assert.That(a.TryInvokeOperator(Operator, out result, a, a));
                VerifyResults(result, a, a, CalculateExpectedResult(Value, Value));
            });
        }

        [Test]
        public void Verify_Homogenous_op_Subtraction_Assignment_Quantity_IQuantity()
        {
            VerifyHomogeneousDimensions(A, A);

            this.DoesNotThrow<IDEX>(() =>
            {
                Quantity result;
                var a = (Quantity) A;
                Assert.That(a.TryInvokeOperator(Operator, out result, a, A));
                VerifyResults(result, a, A, CalculateExpectedResult(Value, Value));
            });
        }

        [Test]
        public void Verify_Incompatible_Heterogeneous_op_Subtraction_Assignment_Quantity_Quantity()
        {
            VerifyHeterogeneousDimensions(A, C);
            var a = (Quantity) A;
            this.Throws<IDEX>(() => a.InvokeOperator<Quantity, Quantity>(Operator, a, (Quantity) C));
        }

        [Test]
        public void Verify_Incompatible_Heterogeneous_op_Subtraction_Assignment_Quantity_IQuantity()
        {
            VerifyHeterogeneousDimensions(A, C);
            var a = (Quantity) A;
            this.Throws<IDEX>(() => a.InvokeOperator<Quantity, Quantity>(Operator, a, C));
        }

        [Test]
        public void Verify_Incompatible_Heterogeneous_op_Subtraction_Assignment_IQuantity_Quantity()
        {
            VerifyHeterogeneousDimensions(A, C);
            var c = (Quantity) C;
            this.Throws<IDEX>(() => c.InvokeOperator<Quantity, Quantity>(Operator, A, c));
        }

        [Test]
        public void Verify_Incompatible_op_Subtraction_Quantity_double()
        {
            var a = (Quantity) A;
            this.Throws<IDEX>(() => a.InvokeOperator<Quantity, Quantity>(Operator, a, Value));
        }

        [Test]
        public void Verify_Incompatible_op_Subtraction_double_Quantity()
        {
            var a = (Quantity) A;
            this.Throws<IDEX>(() => a.InvokeOperator<Quantity, Quantity>(Operator, Value, a));
        }

        [Test]
        public void Verify_Incompatible_op_Subtraction_Assignment_Quantity_double()
        {
            this.Throws<IDEX>(() => CallOperatorAssignment((Quantity) A, Value, (x, y) => x -= y));
        }
    }
}
