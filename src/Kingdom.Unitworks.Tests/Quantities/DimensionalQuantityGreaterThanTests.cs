using System;
using NUnit.Framework;

namespace Kingdom.Unitworks
{
    public class DimensionalQuantityGreaterThanTests
        : DimensionalQuantityLogicalTestFixtureBase
    {
        protected override OperatorPart Operator
        {
            get { return OperatorPart.GreaterThan; }
        }

        protected override Func<double, double, bool> ExpectedFunction
        {
            get { return (x, y) => x > y; }
        }

        #region Dimensionless Greater Than

        [Test]
        public void Verify_op_GreaterThan_Dimensionless_Quantity_Quantity()
        {
            Verify((Quantity) A, (Quantity) C);
        }

        [Test]
        public void Verify_op_GreaterThan_Dimensionless_Quantity_IQuantity()
        {
            Verify((Quantity) A, C);
        }

        [Test]
        public void Verify_op_GreaterThan_Dimensionless_IQuantity_Quantity()
        {
            Verify(A, (Quantity) C);
        }

        [Test]
        public void Verify_op_GreaterThan_Dimensionless_Quantity_double()
        {
            Verify<Quantity>(Value, C);
        }

        [Test]
        public void Verify_op_GreaterThan_Dimensionless_double_Quantity()
        {
            Verify<Quantity>(A, CValue);
        }

        #endregion

        #region Dimensionless Not Greater Than

        [Test]
        public void Verify_op_GreaterThan_Dimensionless_Quantity_C_Quantity_A_Not()
        {
            Verify((Quantity) C, (Quantity) A);
        }

        [Test]
        public void Verify_op_GreaterThan_Dimensionless_Quantity_C_IQuantity_A_Not()
        {
            Verify((Quantity) C, A);
        }

        [Test]
        public void Verify_op_GreaterThan_Dimensionless_IQuantity_C_Quantity_A_Not()
        {
            Verify(C, (Quantity) A);
        }

        [Test]
        public void Verify_op_GreaterThan_Dimensionless_Quantity_C_double_Value_Not()
        {
            Verify<Quantity>(C, Value);
        }

        [Test]
        public void Verify_op_GreaterThan_Dimensionless_double_CValue_Quantity_A_Not()
        {
            Verify<Quantity>(CValue, A);
        }

        [Test]
        public void Verify_op_GreaterThan_Dimensionless_Quantity_A_Quantity_A_Not()
        {
            Verify((Quantity) A, (Quantity) A);
        }

        [Test]
        public void Verify_op_GreaterThan_Dimensionless_Quantity_A_IQuantity_A_Not()
        {
            Verify((Quantity) A, A);
        }

        [Test]
        public void Verify_op_GreaterThan_Dimensionless_IQuantity_A_Quantity_A_Not()
        {
            Verify(A, (Quantity) A);
        }

        [Test]
        public void Verify_op_GreaterThan_Dimensionless_Quantity_A_double_Value_Not()
        {
            Verify<Quantity>(A, Value);
        }

        [Test]
        public void Verify_op_GreaterThan_Dimensionless_double_Value_Quantity_A_Not()
        {
            Verify<Quantity>(Value, A);
        }

        #endregion

        #region Homogenous Dimensional Greater Than

        [Test]
        public void Verify_op_GreaterThan_Homogenous_Dimensional_Quantity_Quantity()
        {
            Verify((Quantity) D, (Quantity) F);
        }

        [Test]
        public void Verify_op_GreaterThan_Homogenous_Dimensional_Quantity_IQuantity()
        {
            Verify((Quantity) D, F);
        }

        [Test]
        public void Verify_op_GreaterThan_Homogenous_Dimensional_IQuantity_Quantity()
        {
            Verify(D, (Quantity) F);
        }

        [Test]
        public void Verify_op_GreaterThan_Dimensional_Quantity_double()
        {
            Verify<Quantity>(D, Value, false);
        }

        [Test]
        public void Verify_op_GreaterThan_Dimensional_double_Quantity()
        {
            Verify<Quantity>(Value, D, false);
        }

        #endregion

        #region Homogenous Dimensional Not Greater Than

        [Test]
        public void Verify_op_GreaterThan_Homogenous_Dimensional_Quantity_F_Quantity_D_Not()
        {
            Verify((Quantity) F, (Quantity) D);
        }

        [Test]
        public void Verify_op_GreaterThan_Homogenous_Dimensional_Quantity_F_IQuantity_D_Not()
        {
            Verify((Quantity) F, D);
        }

        [Test]
        public void Verify_op_GreaterThan_Homogenous_Dimensional_IQuantity_F_Quantity_D_Not()
        {
            Verify(F, (Quantity) D);
        }


        [Test]
        public void Verify_op_GreaterThan_Homogenous_Dimensional_Quantity_D_Quantity_D_Not()
        {
            Verify((Quantity) D, (Quantity) D);
        }

        [Test]
        public void Verify_op_GreaterThan_Homogenous_Dimensional_Quantity_D_IQuantity_D_Not()
        {
            Verify((Quantity) D, D);
        }

        [Test]
        public void Verify_op_GreaterThan_Homogenous_Dimensional_IQuantity_D_Quantity_D_Not()
        {
            Verify(D, (Quantity) D);
        }

        #endregion

        #region Heterogenous Dimensional Not Greater Than

        [Test]
        public void Verify_op_GreaterThan_Heterogenous_Dimensional_Quantity_A_Quantity_D_Not()
        {
            Verify((Quantity) A, (Quantity) D, false);
        }

        [Test]
        public void Verify_op_GreaterThan_Heterogenous_Dimensional_Quantity_A_IQuantity_D_Not()
        {
            Verify((Quantity) A, D, false);
        }

        [Test]
        public void Verify_op_GreaterThan_Heterogenous_Dimensional_IQuantity_A_Quantity_D_Not()
        {
            Verify(A, (Quantity) D, false);
        }

        #endregion
    }
}
