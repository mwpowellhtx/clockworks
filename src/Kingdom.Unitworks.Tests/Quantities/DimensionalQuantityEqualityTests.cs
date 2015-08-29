using System;
using NUnit.Framework;

namespace Kingdom.Unitworks
{
    public class DimensionalQuantityEqualityTests
        : DimensionalQuantityLogicalTestFixtureBase
    {
        protected override OperatorPart Operator
        {
            get { return OperatorPart.Equality; }
        }

        protected override Func<double, double, bool> ExpectedFunction
        {
            get { return (x, y) => x.Equals(y); }
        }

        #region Dimensionless Equals

        [Test]
        public void Verify_op_Equality_Dimensionless_Quantity_Quantity()
        {
            Verify((Quantity) A, (Quantity) B);
        }

        [Test]
        public void Verify_op_Equality_Dimensionless_Quantity_IQuantity()
        {
            Verify((Quantity) A, B);
        }

        [Test]
        public void Verify_op_Equality_Dimensionless_IQuantity_Quantity()
        {
            Verify(A, (Quantity) B);
        }

        [Test]
        public void Verify_op_Equality_Dimensionless_Quantity_double()
        {
            Verify<Quantity>(A, Value, true);
        }

        [Test]
        public void Verify_op_Equality_Dimensionless_double_Quantity()
        {
            Verify<Quantity>(Value, A, true);
        }

        #endregion

        #region Dimensionless Does Not Equal

        [Test]
        public void Verify_op_Equality_Dimensionless_Quantity_Quantity_Not()
        {
            Verify((Quantity) A, (Quantity) C);
        }

        [Test]
        public void Verify_op_Equality_Dimensionless_Quantity_IQuantity_Not()
        {
            Verify((Quantity) A, C);
        }

        [Test]
        public void Verify_op_Equality_Dimensionless_IQuantity_Quantity_Not()
        {
            Verify(A, (Quantity) C);
        }

        [Test]
        public void Verify_op_Equality_Dimensionless_Quantity_double_Not()
        {
            Verify<Quantity>(A, CValue, false);
        }

        [Test]
        public void Verify_op_Equality_Dimensionless_double_Quantity_Not()
        {
            Verify<Quantity>(CValue, A, false);
        }

        #endregion

        #region Homogenous Dimensional Does Not Equal

        [Test]
        public void Verify_op_Equality_Homogenous_Dimensional_Quantity_Quantity()
        {
            Verify((Quantity) D, (Quantity) E);
        }

        [Test]
        public void Verify_op_Equality_Homogeneous_Dimensional_Quantity_IQuantity()
        {
            Verify((Quantity) D, E);
        }

        [Test]
        public void Verify_op_Equality_Homogeneous_Dimensional_IQuantity_Quantity()
        {
            Verify(D, (Quantity) E);
        }

        [Test]
        public void Verify_op_Equality_Dimensional_Quantity_double()
        {
            Verify<Quantity>(D, DValue, false);
        }

        [Test]
        public void Verify_op_Equality_Dimensional_double_Quantity()
        {
            Verify<Quantity>(DValue, D, false);
        }

        #endregion

        #region Homogenous Dimensional Does Not Equal

        [Test]
        public void Verify_op_Equality_Homogeneous_Dimensional_Quantity_Quantity_Not()
        {
            Verify((Quantity) D, (Quantity) F);
        }

        [Test]
        public void Verify_op_Equality_Homogeneous_Dimensional_Quantity_IQuantity_Not()
        {
            Verify((Quantity) D, F);
        }

        [Test]
        public void Verify_op_Equality_Homogeneous_Dimensional_IQuantity_Quantity_Not()
        {
            Verify(D, (Quantity) F);
        }

        #endregion

        #region Heterogeneous Dimensional Equals

        [Test]
        public void Verify_op_Equality_Heterogeneous_Dimensional_Quantity_D_Quantity_E()
        {
            Verify((Quantity) D, (Quantity) E);
        }

        [Test]
        public void Verify_op_Equality_Heterogeneous_Dimensional_Quantity_D_IQuantity_E()
        {
            Verify((Quantity) D, E);
        }

        [Test]
        public void Verify_op_Equality_Heterogeneous_Dimensional_IQuantity_D_Quantity_E()
        {
            Verify(D, (Quantity) E);
        }

        [Test]
        public void Verify_op_Equality_Heterogeneous_Dimensional_Quantity_E_Quantity_D()
        {
            Verify((Quantity) E, (Quantity) D);
        }

        [Test]
        public void Verify_op_Equality_Heterogeneous_Dimensional_Quantity_E_IQuantity_D()
        {
            Verify((Quantity) E, D);
        }

        [Test]
        public void Verify_op_Equality_Heterogeneous_Dimensional_IQuantity_E_Quantity_D()
        {
            Verify(E, (Quantity) D);
        }

        #endregion

        #region Heterogeneous Dimensional Does Not Equal

        [Test]
        public void Verify_op_Equality_Heterogeneous_Dimensional_Quantity_A_Quantity_D_Not()
        {
            Verify((Quantity) A, (Quantity) D);
        }

        [Test]
        public void Verify_op_Equality_Heterogeneous_Dimensional_Quantity_A_IQuantity_D_Not()
        {
            Verify((Quantity) A, D);
        }

        [Test]
        public void Verify_op_Equality_Heterogeneous_Dimensional_IQuantity_A_Quantity_D_Not()
        {
            Verify(A, (Quantity) D);
        }

        [Test]
        public void Verify_op_Equality_Heterogeneous_Dimensional_Quantity_D_Quantity_A_Not()
        {
            Verify((Quantity) D, (Quantity) A);
        }

        [Test]
        public void Verify_op_Equality_Heterogeneous_Dimensional_Quantity_D_IQuantity_A_Not()
        {
            Verify((Quantity) D, A);
        }

        [Test]
        public void Verify_op_Equality_Heterogeneous_Dimensional_IQuantity_D_Quantity_A_Not()
        {
            Verify(D, (Quantity) A);
        }

        [Test]
        public void Verify_op_Equality_Dimensional_Quantity_double_Not()
        {
            Verify<Quantity>(D, DValue, false);
        }

        [Test]
        public void Verify_op_Equality_Dimensional_double_Quantity_Not()
        {
            Verify<Quantity>(DValue, D, false);
        }

        #endregion
    }
}
