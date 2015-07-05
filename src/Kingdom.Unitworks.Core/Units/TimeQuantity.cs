using System;

namespace Kingdom.Unitworks.Units
{
    /// <summary>
    /// Represents a quantity in the dimension of time.
    /// </summary>
    public class TimeQuantity : QuantityBase<TimeUnit, double>
    {
        /// <summary>
        /// Gets the BaseUnit, which all calculations wlil be expressed in terms of.
        /// </summary>
        public static TimeUnit BaseUnit
        {
            get { return Converter.BaseUnit; }
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

        /// <summary>
        /// Division operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double operator /(TimeQuantity a, TimeQuantity b)
        {
            var aPart = Converter.Convert(a.Value, a.Unit, BaseUnit);
            var bPart = Converter.Convert(b.Value, b.Unit, BaseUnit);
            return aPart/bPart;
        }

        #endregion

        #region Logical Operator Overloads

        /// <summary>
        /// Equality operator overload.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/53k8ybth.aspx"> == Operator (C# Reference) </a>
        public static bool operator ==(TimeQuantity a, TimeQuantity b)
        {
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
            return a.ToTimeQuantity().Value <= b.ToTimeQuantity().Value;
        }

        #endregion
    }
}
