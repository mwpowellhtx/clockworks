using System;

namespace Kingdom.Unitworks
{
    /// <summary>
    /// 
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
    /// 
    /// </summary>
    /// <typeparam name="TUnit"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public abstract class QuantityBase<TUnit, TValue> : QuantityBase<TValue>
        where TValue : struct, IComparable, IComparable<TValue>, IEquatable<TValue>, IConvertible
    {
        /// <summary>
        /// Unit backing field.
        /// </summary>
        private TUnit _unit;

        /// <summary>
        /// Gets or sets the Unit.
        /// </summary>
        public virtual TUnit Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        /// <summary>
        /// Protected Constructor
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="value"></param>
        protected QuantityBase(TUnit unit, TValue value)
            : base(value)
        {
            Initialize(unit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        private void Initialize(TUnit unit)
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
