using System;

namespace Kingdom.Unitworks.Units
{
    /// <summary>
    /// 
    /// </summary>
    public class MassQuantity : QuantityBase<MassUnit, double>
    {
        /// <summary>
        /// Gets the BaseUnit, which all calculations will be expressed in terms of.
        /// </summary>
        public static MassUnit BaseUnit
        {
            get { return Converter.BaseUnit; }
        }

        protected override MassUnit GetBaseUnit()
        {
            return Converter.BaseUnit;
        }

        /// <summary>
        /// Converter backing field.
        /// </summary>
        protected internal static readonly MassConverter Converter = new MassConverter();

        /// <summary>
        /// Gets or sets the Unit.
        /// </summary>
        public override MassUnit Unit
        {
            get { return base.Unit; }
            set
            {
                if (base.Unit == value) return;
                Value = Converter.Convert(Value, base.Unit, value);
                base.Unit = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="value"></param>
        public MassQuantity(MassUnit unit, double value)
            : base(unit, value)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        public MassQuantity(double value)
            : base(BaseUnit, value)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unit"></param>
        public MassQuantity(MassUnit unit)
            : base(unit, default(double))
        {
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MassQuantity()
            : base(BaseUnit, default(double))
        {
        }

        #region Mathematical Operator Overloads

        /// <summary>
        /// Performs an additive operation on <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="adder"></param>
        /// <returns></returns>
        private static MassQuantity Additive(MassQuantity a, MassQuantity b,
            Func<double, double, double> adder)
        {
            var aPart = Converter.Convert(a.Value, a.Unit, BaseUnit);
            var bPart = Converter.Convert(b.Value, b.Unit, BaseUnit);
            var added = adder(aPart, bPart);
            var converted = Converter.Convert(added, BaseUnit, a.Unit);
            return new MassQuantity(a.Unit, converted);
        }

        /// <summary>
        /// Addition operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static MassQuantity operator +(MassQuantity a, MassQuantity b)
        {
            return Additive(a, b, (x, y) => x + y);
        }

        /// <summary>
        /// Subtraction operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static MassQuantity operator -(MassQuantity a, MassQuantity b)
        {
            return Additive(a, b, (x, y) => x - y);
        }

        /// <summary>
        /// Multiplication operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static MassQuantity operator *(MassQuantity a, double factor)
        {
            return new MassQuantity(a.Unit, a.Value*factor);
        }

        /// <summary>
        /// Multiplication operator overload.
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static MassQuantity operator *(double factor, MassQuantity a)
        {
            return new MassQuantity(a.Unit, factor*a.Value);
        }

        //TODO: in and of itself, not worth repackaging Clockworks; however, these sorts of "patches" might be a sufficient reason to capture "Unitworks" as a separate package apart from Clockworks: just need to be careful to capture the proper reference/versions
        /// <summary>
        /// Division operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <a href="!:http://en.wikipedia.org/wiki/IEEE_floating_point">IEEE floating point</a>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/system.double.negativeinfinity.aspx">System.Double.NegativeInfinity</a>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/system.double.positiveinfinity.aspx">System.Double.PositiveInfinity</a>
        public static double operator /(MassQuantity a, MassQuantity b)
        {
            var aPart = Converter.Convert(a.Value, a.Unit, BaseUnit);
            var bPart = Converter.Convert(b.Value, b.Unit, BaseUnit);
            return aPart/bPart;
        }

        #endregion

        #region Relational Operator Overloads

        /// <summary>
        /// Equality operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/53k8ybth.aspx"> == Operator (C# Reference) </a>
        public static bool operator ==(MassQuantity a, MassQuantity b)
        {
            //TODO: may even want to defer to a Comparable pattern...
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.ToMassQuantity().Value.Equals(b.ToMassQuantity().Value);
        }

        /// <summary>
        /// Inequality operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/3tz250sf.aspx"> != Operator (C# Reference) </a>
        public static bool operator !=(MassQuantity a, MassQuantity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return false;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return true;
            return !a.ToMassQuantity().Value.Equals(b.ToMassQuantity().Value);
        }

        /// <summary>
        /// Greater than relational operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/yxk8751b.aspx"> &gt; Operator (C# Reference) </a>
        public static bool operator >(MassQuantity a, MassQuantity b)
        {
            //TODO: these might even be exceptions... or defer to Comparable...
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.ToMassQuantity().Value > b.ToMassQuantity().Value;
        }

        /// <summary>
        /// Less than relational operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/z5wecxwa.aspx"> &lt; Operator (C# Reference) </a>
        public static bool operator <(MassQuantity a, MassQuantity b)
        {
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.ToMassQuantity().Value < b.ToMassQuantity().Value;
        }

        /// <summary>
        /// Greater than or equal to relational operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/a59bsyk4.aspx"> &gt;= Operator (C# Reference) </a>
        public static bool operator >=(MassQuantity a, MassQuantity b)
        {
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.ToMassQuantity().Value >= b.ToMassQuantity().Value;
        }

        /// <summary>
        /// Less than relational operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/hx063734.aspx"> &lt;= Operator (C# Reference) </a>
        public static bool operator <=(MassQuantity a, MassQuantity b)
        {
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.ToMassQuantity().Value <= b.ToMassQuantity().Value;
        }

        #endregion

        #region Cloneable Members

        /// <summary>
        /// Returns a Clone of this object.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new MassQuantity(Unit, Value);
        }

        #endregion
    }
}
