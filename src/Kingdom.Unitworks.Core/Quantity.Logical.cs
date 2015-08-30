using System;
using Kingdom.Unitworks.Dimensions;

namespace Kingdom.Unitworks
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Quantity : IEquatable<Quantity>, IComparable<Quantity>
    {
        #region Equatable Members

        /// <summary>
        /// Returns whether <paramref name="x"/> Equals <paramref name="y"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool Equals(IQuantity x, IQuantity y)
        {
            if (ReferenceEquals(x, y)) return true;

            if (x == null || y == null) return false;

            if (!x.Dimensions.AreCompatible(y.Dimensions))
                return false;

            var xBase = x.ToBase();
            var yBase = y.ToBase();

            return xBase.Value.Equals(yBase.Value);
        }

        /// <summary>
        /// Returns whether this quantity Equals the <paramref name="otherQty"/>.
        /// </summary>
        /// <param name="otherQty"></param>
        /// <returns></returns>
        public bool Equals(IQuantity otherQty)
        {
            return Equals(this, otherQty);
        }

        /// <summary>
        /// Returns whether this quantity Equals the <paramref name="otherQty"/>.
        /// </summary>
        /// <param name="otherQty"></param>
        /// <returns></returns>
        public bool Equals(Quantity otherQty)
        {
            return Equals(this, otherQty);
        }

        #endregion

        #region Comparable Members

        private static bool TryCompareTo(IQuantity x, IQuantity y, bool xLess, bool yLess, bool equals,
            bool incompatible = false)
        {
            switch (CompareTo(x, y))
            {
                case 0:
                    return equals;
                case -1:
                    return xLess;
                case 1:
                    return yLess;
                case -2:
                    return incompatible;
            }

            throw new InvalidOperationException("Invalid operation comparing x with y");
        }

        /// <summary>
        /// Returns the comparison between <paramref name="x"/> and <paramref name="y"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int CompareTo(IQuantity x, IQuantity y)
        {
            //TODO: can Dimensions really be all that comparable?

            if (x != null && y == null) return 1;
            if (y != null && x == null) return -1;
            if (x == null) return 0;

            ////TODO: TBD: whether to verify here? throw exception? or just take the "silent" result as in TryCompareTo
            //x.VerifyEquivalent(y);

            if (!x.Dimensions.AreCompatible(y.Dimensions))
                return -2;

            var xBase = x.ToBase();
            var yBase = y.ToBase();

            return xBase.Value.CompareTo(yBase.Value);
        }

        /// <summary>
        /// Returns the comparison between this and the <paramref name="otherQty"/>.
        /// </summary>
        /// <param name="otherQty"></param>
        /// <returns></returns>
        public int CompareTo(IQuantity otherQty)
        {
            return CompareTo(this, otherQty);
        }

        /// <summary>
        /// Returns the comparison between this and the <paramref name="otherQty"/>.
        /// </summary>
        /// <param name="otherQty"></param>
        /// <returns></returns>
        public int CompareTo(Quantity otherQty)
        {
            return CompareTo(this, otherQty);
        }

        #endregion

        #region Logical Operator Support Members

        private static bool Equality(IQuantity x, IQuantity y)
        {
            return TryCompareTo(x, y, false, false, true);
        }

        private static bool LessThan(IQuantity x, IQuantity y)
        {
            return TryCompareTo(x, y, true, false, false);
        }

        private static bool GreaterThan(IQuantity x, IQuantity y)
        {
            return TryCompareTo(x, y, false, true, false);
        }

        private static bool LessThanOrEqual(IQuantity x, IQuantity y)
        {
            return TryCompareTo(x, y, true, false, true);
        }

        private static bool GreaterThanOrEqual(IQuantity x, IQuantity y)
        {
            return TryCompareTo(x, y, false, true, true);
        }

        #endregion

        #region Quantity == Quantity

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(Quantity x, Quantity y)
        {
            return Equality(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(Quantity x, Quantity y)
        {
            return !Equality(x, y);
        }

        #endregion

        #region Quantity == IQuantity

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(Quantity x, IQuantity y)
        {
            return Equality(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(Quantity x, IQuantity y)
        {
            return !Equality(x, y);
        }

        #endregion

        #region IQuantity == Quantity

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(IQuantity x, Quantity y)
        {
            return Equality(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(IQuantity x, Quantity y)
        {
            return !Equality(x, y);
        }

        #endregion

        #region Quantity == double (Dimensionless)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(Quantity x, double y)
        {
            return Equality(x, new Quantity(y));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(Quantity x, double y)
        {
            return !Equality(x, new Quantity(y));
        }

        #endregion

        #region double (Dimensionless) == Quantity

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(double x, Quantity y)
        {
            return Equality(new Quantity(x), y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(double x, Quantity y)
        {
            return !Equality(new Quantity(x), y);
        }

        #endregion

        #region Quantity < Quantity

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator <(Quantity x, Quantity y)
        {
            return LessThan(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator >(Quantity x, Quantity y)
        {
            return GreaterThan(x, y);
        }

        #endregion

        #region Quantity < IQuantity

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator <(Quantity x, IQuantity y)
        {
            return LessThan(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator >(Quantity x, IQuantity y)
        {
            return GreaterThan(x, y);
        }

        #endregion

        #region IQuantity < Quantity

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator <(IQuantity x, Quantity y)
        {
            return LessThan(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator >(IQuantity x, Quantity y)
        {
            return GreaterThan(x, y);
        }

        #endregion

        #region Quantity < double (Dimensionless)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator <(Quantity x, double y)
        {
            return LessThan(x, new Quantity(y));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator >(Quantity x, double y)
        {
            return GreaterThan(x, new Quantity(y));
        }

        #endregion

        #region double (Dimensionless) < Quantity

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator <(double x, Quantity y)
        {
            return LessThan(new Quantity(x), y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator >(double x, Quantity y)
        {
            return GreaterThan(new Quantity(x), y);
        }

        #endregion

        #region Quantity <= Quantity

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator <=(Quantity x, Quantity y)
        {
            return LessThanOrEqual(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator >=(Quantity x, Quantity y)
        {
            return GreaterThanOrEqual(x, y);
        }

        #endregion

        #region Quantity <= IQuantity

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator <=(Quantity x, IQuantity y)
        {
            return LessThanOrEqual(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator >=(Quantity x, IQuantity y)
        {
            return GreaterThanOrEqual(x, y);
        }

        #endregion

        #region IQuantity <= Quantity

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator <=(IQuantity x, Quantity y)
        {
            return LessThanOrEqual(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator >=(IQuantity x, Quantity y)
        {
            return GreaterThanOrEqual(x, y);
        }

        #endregion

        #region Quantity <= double (Dimensionless)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator <=(double x, Quantity y)
        {
            return LessThanOrEqual(new Quantity(x), y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator >=(double x, Quantity y)
        {
            return GreaterThanOrEqual(new Quantity(x), y);
        }

        #endregion

        #region Quantity <= double (Dimensionless)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator <=(Quantity x, double y)
        {
            return LessThanOrEqual(x, new Quantity(y));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator >=(Quantity x, double y)
        {
            return GreaterThanOrEqual(x, new Quantity(y));
        }

        #endregion
    }
}
