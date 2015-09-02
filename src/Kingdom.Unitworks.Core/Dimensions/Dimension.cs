using System;
using System.Linq;
using System.Reflection;

namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// Represents what a dimension is to quantities and dimensional analysis.
    /// </summary>
    public abstract partial class Dimension : IDimension
    {
        public override int GetHashCode()
        {
            // So that regardless of the unit, only one can be present.
            return DimensionId.GetHashCode();
        }

        public abstract IDimension GetBase();

        protected internal static IDimension GetBase<TDimension, TInterface>()
            where TInterface : IDimension
            where TDimension : Dimension, TInterface
        {
            const BindingFlags flags
                = BindingFlags.Public
                  | BindingFlags.Static
                  | BindingFlags.GetField
                  | BindingFlags.DeclaredOnly;

            var fieldInfos = typeof (TDimension).GetFields(flags).ToArray();

            var fields = fieldInfos.Select(f => f.GetValue(null)).OfType<TInterface>().ToArray();

            var baseUnit = fields.SingleOrDefault(f => f != null && f.IsBaseUnit);

            return baseUnit;
        }

        public abstract bool IsBaseUnit { get; }

        private int _exponent;

        public virtual int Exponent
        {
            get { return _exponent; }
            set { _exponent = value; }
        }

        public string Abbreviation { get; protected internal set; }

        private string _displayName;

        public string DisplayName
        {
            get { return _displayName ?? (_displayName = GetFieldName(GetType(), this)); }
            protected internal set { _displayName = value; }
        }

        /// <summary>
        /// Protected Constructor
        /// </summary>
        /// <param name="abbreviation"></param>
        protected Dimension(string abbreviation)
        {
            Register(this);
            _exponent = 1;
            Abbreviation = abbreviation;
        }

        /// <summary>
        /// Protected Constructor
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <param name="toBase"></param>
        /// <param name="fromBase"></param>
        protected Dimension(string abbreviation, IUnitConversion toBase = null, IUnitConversion fromBase = null)
            : this(abbreviation)
        {
            ToBase = toBase ?? BaseDimensionUnitConversion.DefaultConversion;
            FromBase = fromBase ?? BaseDimensionUnitConversion.DefaultConversion;
        }

        /// <summary>
        /// Protected Constructor
        /// </summary>
        /// <param name="other"></param>
        protected Dimension(Dimension other)
            : this(other.Abbreviation)
        {
            _exponent = other.Exponent;
            DisplayName = other.DisplayName;
            ToBase = other.ToBase ?? BaseDimensionUnitConversion.DefaultConversion;
            FromBase = other.FromBase ?? BaseDimensionUnitConversion.DefaultConversion;
        }

        private static string GetFieldName(Type parentType, object instance)
        {
            const BindingFlags flags
                = BindingFlags.Public
                  | BindingFlags.Static
                  | BindingFlags.GetField
                  | BindingFlags.DeclaredOnly;

            // TODO: what I wouldn't give for the nameof operator ... oh wait, VS2015 "Roslyn" does introduce this ...
            var fieldInfo = parentType.GetFields(flags).SingleOrDefault(
                x => ReferenceEquals(x.GetValue(null), instance));

            if (ReferenceEquals(fieldInfo, null))
            {
                var message = string.Format("Unable to locate field in parent type {0}", parentType);
                throw new ArgumentException(message, "parentType");
            }

            return fieldInfo.Name;
        }

        public IUnitConversion ToBase { get; protected set; }

        public IUnitConversion FromBase { get; protected set; }

        public double ConvertToBase(double value)
        {
            return ToBase.Convert(value, Exponent);
        }

        public double ConvertFromBase(double value)
        {
            return FromBase.Convert(value, Exponent);
        }

        public virtual IDimension Invert()
        {
            var inverted = (IDimension) Clone();
            inverted.Exponent = 0 - Exponent;
            return inverted;
        }

        public virtual IDimension Squared()
        {
            var exponent = Exponent;
            var squared = (IDimension) Clone();
            squared.Exponent = exponent + exponent;
            return squared;
        }

        public virtual IDimension Cubed()
        {
            var exponent = Exponent;
            var cubed = (IDimension) Clone();
            cubed.Exponent = exponent + exponent + exponent;
            return cubed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        public virtual IDimension Power(int power)
        {
            var exponent = Exponent;
            var cloned = (IDimension) Clone();
            cloned.Exponent = exponent*power;
            return cloned;
        }

        protected internal static bool Equals(IDimension a, IDimension b)
        {
            return !(a == null || b == null)
                   && a.IsDimensionEquals(b)
                   && a.Exponent == b.Exponent;
        }

        public virtual bool Equals(IDimension other)
        {
            return Equals(this, other);
        }

        public string ExponentText
        {
            get
            {
                var text = string.Empty;
                if (_exponent < 0 || _exponent > 1)
                    text = string.Format("^{0}", _exponent);
                return text;
            }
        }

        public override string ToString()
        {
            var formatted = (Abbreviation ?? string.Empty) + ExponentText;
            return formatted;
        }

        public abstract object Clone();
    }
}
