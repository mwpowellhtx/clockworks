using System;

namespace Kingdom.Unitworks
{
    /// <summary>
    /// Represents a dimensionless <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class QuantityBase<TValue>
    {
        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        public virtual TValue Value { get; set; }

        /// <summary>
        /// Protected Constructor
        /// </summary>
        /// <param name="value"></param>
        protected QuantityBase(TValue value)
        {
            Initialize(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private void Initialize(TValue value)
        {
            Value = value;
        }

        /// <summary>
        /// Protected Constructor
        /// </summary>
        protected QuantityBase()
            : this(default(TValue))
        {
        }

        /// <summary>
        /// Returns the string representation of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(@"{0}", Value);
        }
    }

    /// <summary>
    /// Represents some measurable <typeparamref name="TValue"/> in a <typeparamref name="TDimension"/>.
    /// </summary>
    /// <typeparam name="TDimension">Typically provided as as an enumerated type, the values of which are considered the units.</typeparam>
    /// <typeparam name="TValue">Represents the value of the quantity itself.</typeparam>
    public abstract class QuantityBase<TDimension, TValue> : QuantityBase<TValue>
        where TValue : struct, IComparable, IComparable<TValue>, IEquatable<TValue>, IConvertible
    {
        /// <summary>
        /// Unit backing field.
        /// </summary>
        private TDimension _unit;

        /// <summary>
        /// Gets or sets the Unit.
        /// </summary>
        public virtual TDimension Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        /// <summary>
        /// Protected Constructor
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="value"></param>
        protected QuantityBase(TDimension unit, TValue value)
            : base(value)
        {
            Initialize(unit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        private void Initialize(TDimension unit)
        {
            _unit = unit;
        }

        /// <summary>
        /// Returns the string representation of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(@"{0} {1}", Value, Unit);
        }
    }
}
