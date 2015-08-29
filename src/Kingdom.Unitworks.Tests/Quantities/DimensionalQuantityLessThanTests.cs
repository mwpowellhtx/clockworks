using System;
using NUnit.Framework;

namespace Kingdom.Unitworks
{
    public class DimensionalQuantityLessThanTests
        : DimensionalQuantityLogicalTestFixtureBase
    {
        protected override OperatorPart Operator
        {
            get { return OperatorPart.LessThan; }
        }

        protected override Func<double, double, bool> ExpectedFunction
        {
            get { return (x, y) => x < y; }
        }

        #region Dimensionless Less Than

        [Test]
        public void Verify_op_LessThan_Dimensionless_Quantity_C_Quantity_A()
        {
            Verify((Quantity) C, (Quantity) A);
        }

        [Test]
        public void Verify_op_LessThan_Dimensionless_Quantity_C_IQuantity_A()
        {
            Verify((Quantity) C, A);
        }

        [Test]
        public void Verify_op_LessThan_Dimensionless_IQuantity_C_Quantity_A()
        {
            Verify(C, (Quantity) A);
        }

        [Test]
        public void Verify_op_LessThan_Dimensionless_Quantity_C_double_Value()
        {
            Verify<Quantity>(C, Value);
        }

        [Test]
        public void Verify_op_LessThan_Dimensionless_double_CValue_Quantity_A()
        {
            Verify<Quantity>(CValue, A);
        }

        #endregion

        #region Dimensionless Not Less Than

        [Test]
        public void Verify_op_LessThan_Dimensionless_Quantity_A_Quantity_C_Not()
        {
            Verify((Quantity) A, (Quantity) C);
        }

        [Test]
        public void Verify_op_LessThan_Dimensionless_Quantity_A_IQuantity_C_Not()
        {
            Verify((Quantity) A, C);
        }

        [Test]
        public void Verify_op_LessThan_Dimensionless_IQuantity_A_Quantity_C_Not()
        {
            Verify(A, (Quantity) C);
        }

        [Test]
        public void Verify_op_LessThan_Dimensionless_Quantity_A_double_CValue_Not()
        {
            Verify<Quantity>(A, CValue);
        }

        [Test]
        public void Verify_op_LessThan_Dimensionless_double_Value_Quantity_C_Not()
        {
            Verify<Quantity>(Value, C);
        }

        [Test]
        public void Verify_op_LessThan_Dimensionless_Quantity_A_Quantity_A_Not()
        {
            Verify((Quantity) A, (Quantity) A);
        }

        [Test]
        public void Verify_op_LessThan_Dimensionless_Quantity_A_IQuantity_A_Not()
        {
            Verify((Quantity) A, A);
        }

        [Test]
        public void Verify_op_LessThan_Dimensionless_IQuantity_A_Quantity_A_Not()
        {
            Verify(A, (Quantity) A);
        }

        [Test]
        public void Verify_op_LessThan_Dimensionless_Quantity_A_double_Value_Not()
        {
            Verify<Quantity>(A, Value);
        }

        [Test]
        public void Verify_op_LessThan_Dimensionless_double_Value_Quantity_A_Not()
        {
            Verify<Quantity>(Value, A);
        }

        #endregion

        #region Homogenous Dimensional Less Than

        [Test]
        public void Verify_op_LessThan_Homogenous_Dimensional_Quantity_F_Quantity_D()
        {
            Verify((Quantity) F, (Quantity) D);
        }

        [Test]
        public void Verify_op_LessThan_Homogenous_Dimensional_Quantity_F_IQuantity_D()
        {
            Verify((Quantity) F, D);
        }

        [Test]
        public void Verify_op_LessThan_Homogenous_Dimensional_IQuantity_F_Quantity_D()
        {
            Verify(F, (Quantity) D);
        }

        [Test]
        public void Verify_op_LessThan_Dimensional_Quantity_D_double_Value()
        {
            Verify<Quantity>(D, Value, false);
        }

        [Test]
        public void Verify_op_LessThan_Dimensional_double_Value_Quantity_D()
        {
            Verify<Quantity>(Value, D, false);
        }

        #endregion

        #region Homogenous Dimensional Not Less Than

        [Test]
        public void Verify_op_LessThan_Homogenous_Dimensional_Quantity_D_Quantity_F_Not()
        {
            Verify((Quantity) D, (Quantity) F);
        }

        [Test]
        public void Verify_op_LessThan_Homogenous_Dimensional_Quantity_D_IQuantity_F_Not()
        {
            Verify((Quantity) D, F);
        }

        [Test]
        public void Verify_op_LessThan_Homogenous_Dimensional_IQuantity_D_Quantity_F_Not()
        {
            Verify(D, (Quantity) F);
        }

        [Test]
        public void Verify_op_LessThan_Homogenous_Dimensional_Quantity_D_Quantity_D_Not()
        {
            Verify((Quantity) D, (Quantity) D);
        }

        [Test]
        public void Verify_op_LessThan_Homogenous_Dimensional_Quantity_D_IQuantity_D_Not()
        {
            Verify((Quantity) D, D);
        }

        [Test]
        public void Verify_op_LessThan_Homogenous_Dimensional_IQuantity_D_Quantity_D_Not()
        {
            Verify(D, (Quantity) D);
        }

        #endregion

        #region Heterogenous Dimensional Not Less Than

        [Test]
        public void Verify_op_LessThan_Heterogenous_Dimensional_Quantity_A_Quantity_D_Not()
        {
            Verify((Quantity) A, (Quantity) D, false);
        }

        [Test]
        public void Verify_op_LessThan_Heterogenous_Dimensional_Quantity_A_IQuantity_D_Not()
        {
            Verify((Quantity) A, D, false);
        }

        [Test]
        public void Verify_op_LessThan_Heterogenous_Dimensional_IQuantity_A_Quantity_D_Not()
        {
            Verify(A, (Quantity) D, false);
        }

        #endregion
    }
}
