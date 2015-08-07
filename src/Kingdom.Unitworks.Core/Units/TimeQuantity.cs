using System;

namespace Kingdom.Unitworks.Units
{
    //TODO: Equals(object) ? GetHashCode() ?
    /// <summary>
    /// Represents a quantity in the dimension of time.
    /// </summary>
    public class TimeQuantity : QuantityBase<TimeUnit, double>
    {
        /// <summary>
        /// Gets the BaseUnit, which all calculations will be expressed in terms of.
        /// </summary>
        public static TimeUnit BaseUnit
        {
            get { return Converter.BaseUnit; }
        }

        /// <summary>
        /// Gets the BaseUnit.
        /// </summary>
        /// <returns></returns>
        protected override TimeUnit GetBaseUnit()
        {
            return Converter.BaseUnit;
        }

        /// <summary>
        /// Converter backing field.
        /// </summary>
        protected internal static readonly TimeConverter Converter = new TimeConverter();

        /// <summary>
        /// Gets or sets the Unit.
        /// </summary>
        public override TimeUnit Unit
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
        public TimeQuantity(TimeUnit unit, double value)
            : base(unit, value)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        public TimeQuantity(double value)
            : base(BaseUnit, value)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unit"></param>
        public TimeQuantity(TimeUnit unit)
            : base(unit, default(double))
        {
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TimeQuantity()
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
        private static TimeQuantity Additive(TimeQuantity a, TimeQuantity b,
            Func<double, double, double> adder)
        {
            var aPart = Converter.Convert(a.Value, a.Unit, BaseUnit);
            var bPart = Converter.Convert(b.Value, b.Unit, BaseUnit);
            var added = adder(aPart, bPart);
            var converted = Converter.Convert(added, BaseUnit, a.Unit);
            return new TimeQuantity(a.Unit, converted);
        }

        /// <summary>
        /// Addition operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static TimeQuantity operator +(TimeQuantity a, TimeQuantity b)
        {
            return Additive(a, b, (x, y) => x + y);
        }

        /// <summary>
        /// Subtraction operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static TimeQuantity operator -(TimeQuantity a, TimeQuantity b)
        {
            return Additive(a, b, (x, y) => x - y);
        }

        /// <summary>
        /// Multiplication operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static TimeQuantity operator *(TimeQuantity a, double factor)
        {
            return new TimeQuantity(a.Unit, a.Value*factor);
        }

        /// <summary>
        /// Multiplication operator overload.
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static TimeQuantity operator *(double factor, TimeQuantity a)
        {
            return new TimeQuantity(a.Unit, factor*a.Value);
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
        public static double operator /(TimeQuantity a, TimeQuantity b)
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
        public static bool operator ==(TimeQuantity a, TimeQuantity b)
        {
            //TODO: may even want to defer to a Comparable pattern...
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.ToTimeQuantity().Value.Equals(b.ToTimeQuantity().Value);
        }

        /// <summary>
        /// Inequality operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/3tz250sf.aspx"> != Operator (C# Reference) </a>
        public static bool operator !=(TimeQuantity a, TimeQuantity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return false;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return true;
            return !a.ToTimeQuantity().Value.Equals(b.ToTimeQuantity().Value);
        }

        /// <summary>
        /// Greater than relational operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/yxk8751b.aspx"> &gt; Operator (C# Reference) </a>
        public static bool operator >(TimeQuantity a, TimeQuantity b)
        {
            //TODO: these might even be exceptions... or defer to Comparable...
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.ToTimeQuantity().Value > b.ToTimeQuantity().Value;
        }

        /// <summary>
        /// Less than relational operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/z5wecxwa.aspx"> &lt; Operator (C# Reference) </a>
        public static bool operator <(TimeQuantity a, TimeQuantity b)
        {
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.ToTimeQuantity().Value < b.ToTimeQuantity().Value;
        }

        /// <summary>
        /// Greater than or equal to relational operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/a59bsyk4.aspx"> &gt;= Operator (C# Reference) </a>
        public static bool operator >=(TimeQuantity a, TimeQuantity b)
        {
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.ToTimeQuantity().Value >= b.ToTimeQuantity().Value;
        }

        /// <summary>
        /// Less than relational operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/hx063734.aspx"> &lt;= Operator (C# Reference) </a>
        public static bool operator <=(TimeQuantity a, TimeQuantity b)
        {
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.ToTimeQuantity().Value <= b.ToTimeQuantity().Value;
        }

        #endregion

        #region Cloneable Members

        /// <summary>
        /// Returns a Clone of this object.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new TimeQuantity(Unit, Value);
        }

        #endregion
    }
}
