using System;
using Kingdom.Unitworks.Dimensions;
using NUnit.Framework;

namespace Kingdom.Unitworks.Trigonometric
{
    internal class TrigonometricPart : ITrigonometricPart
    {
        private readonly ITrigonometryContext _context;

        private readonly double _value;

        private readonly IDimension[] _dimensions;

        private readonly Func<double, IDimension[], IQuantity> _factory;

        private IQuantity _qty;

        public IQuantity Qty
        {
            get
            {
                if (ReferenceEquals(null, _qty))
                {
                    _qty = _factory(_value, _dimensions);
                    Assert.That(_qty, Is.Not.Null);
                    Assert.That(_qty.Dimensions.AreEquivalent(_dimensions));
                    Assert.That(_qty.Value, _context.HasEpsilon
                        ? Is.EqualTo(_value).Within(_context.Epsilon)
                        : Is.EqualTo(_value));
                }
                return _qty;
            }
        }

        internal TrigonometricPart(ITrigonometryContext context, double value,
            Func<double, IDimension[], IQuantity> factory, params IDimension[] dimensions)
        {
            Assert.That(_context = context, Is.Not.Null);
            // Some use cases involve NaN.
            _value = value;
            Assert.That(_dimensions = dimensions, Is.Not.Null);
            Assert.That(_factory = factory, Is.Not.Null);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
