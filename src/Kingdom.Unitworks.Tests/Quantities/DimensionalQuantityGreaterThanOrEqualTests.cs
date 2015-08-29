using System;
using NUnit.Framework;

namespace Kingdom.Unitworks
{
    public class DimensionalQuantityGreaterThanOrEqualTests
        : DimensionalQuantityLogicalTestFixtureBase
    {
        protected override OperatorPart Operator
        {
            get { return OperatorPart.GreaterThanOrEqual; }
        }

        protected override Func<double, double, bool> ExpectedFunction
        {
            get { return (x, y) => x >= y; }
        }

        #region Dimensionless Greater Than Or Equal

        [Test]
        public void Verify_op_GreaterThanOrEqual_Dimensionless_Quantity_A_Quantity_C()
        {
            Verify((Quantity) A, (Quantity) C);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Dimensionless_Quantity_A_IQuantity_C()
        {
            Verify((Quantity) A, C);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Dimensionless_IQuantity_A_Quantity_C()
        {
            Verify(A, (Quantity) C);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Dimensionless_Quantity_Value_double_C()
        {
            Verify<Quantity>(Value, C);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Dimensionless_double_A_Quantity_CValue()
        {
            Verify<Quantity>(A, CValue);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Dimensionless_Quantity_A_Quantity_A()
        {
            Verify((Quantity) A, (Quantity) A);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Dimensionless_Quantity_A_IQuantity_A()
        {
            Verify((Quantity) A, A);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Dimensionless_IQuantity_A_Quantity_A()
        {
            Verify(A, (Quantity) A);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Dimensionless_Quantity_A_double_Value()
        {
            Verify<Quantity>(A, Value);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Dimensionless_double_Value_Quantity_A()
        {
            Verify<Quantity>(Value, A);
        }

        #endregion

        #region Dimensionless Not Greater Than Or Equal

        [Test]
        public void Verify_op_GreaterThanOrEqual_Dimensionless_Quantity_C_Quantity_A_Not()
        {
            Verify((Quantity) C, (Quantity) A);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Dimensionless_Quantity_C_IQuantity_A_Not()
        {
            Verify((Quantity) C, A);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Dimensionless_IQuantity_C_Quantity_A_Not()
        {
            Verify(C, (Quantity) A);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Dimensionless_Quantity_CValue_double_A_Not()
        {
            Verify<Quantity>(CValue, A);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Dimensionless_double_C_Quantity_Value_Not()
        {
            Verify<Quantity>(C, Value);
        }

        #endregion

        #region Homogenous Dimensional Greater Than Or Equal

        [Test]
        public void Verify_op_GreaterThanOrEqual_Homogenous_Dimensional_Quantity_D_Quantity_F()
        {
            Verify((Quantity) D, (Quantity) F);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Homogenous_Dimensional_Quantity_D_IQuantity_F()
        {
            Verify((Quantity) D, F);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Homogenous_Dimensional_IQuantity_D_Quantity_F()
        {
            Verify(D, (Quantity) F);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Dimensional_Quantity_Value_double_D()
        {
            Verify<Quantity>(Value, D, false);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Dimensional_double_D_Quantity_Value()
        {
            Verify<Quantity>(D, Value, false);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Homogenous_Dimensional_Quantity_D_Quantity_D()
        {
            Verify((Quantity) D, (Quantity) D);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Homogenous_Dimensional_Quantity_D_IQuantity_D()
        {
            Verify((Quantity) D, D);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Homogenous_Dimensional_IQuantity_D_Quantity_D()
        {
            Verify(D, (Quantity) D);
        }

        #endregion

        #region Homogenous Dimensional Not Greater Than Or Equal

        [Test]
        public void Verify_op_GreaterThanOrEqual_Homogenous_Dimensional_Quantity_F_Quantity_D_Not()
        {
            Verify((Quantity) F, (Quantity) D);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Homogenous_Dimensional_Quantity_F_IQuantity_D_Not()
        {
            Verify((Quantity) F, D);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Homogenous_Dimensional_IQuantity_F_Quantity_D_Not()
        {
            Verify(F, (Quantity) D);
        }

        #endregion

        #region Heterogenous Dimensional Not Greater Than Or Equal

        [Test]
        public void Verify_op_GreaterThanOrEqual_Heterogenous_Dimensional_Quantity_A_Quantity_D_Not()
        {
            Verify((Quantity) A, (Quantity) D, false);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Heterogenous_Dimensional_Quantity_A_IQuantity_D_Not()
        {
            Verify((Quantity) A, D, false);
        }

        [Test]
        public void Verify_op_GreaterThanOrEqual_Heterogenous_Dimensional_IQuantity_A_Quantity_D_Not()
        {
            Verify(A, (Quantity) D, false);
        }

        #endregion
    }
}
