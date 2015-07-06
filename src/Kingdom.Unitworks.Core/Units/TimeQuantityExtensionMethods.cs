using System;

namespace Kingdom.Unitworks.Units
{
    /// <summary>
    /// Time quantity extension methods.
    /// </summary>
    public static class TimeQuantityExtensionMethods
    {
        /// <summary>
        /// Returns a new <see cref="TimeQuantity"/> corresponding to the <paramref name="unit"/> and <paramref name="value"/>.
        /// The <see cref="TimeQuantity.BaseUnit"/> is assumed when unit is Null.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static TimeQuantity ToTimeQuantity(this double value, TimeUnit? unit = null)
        {
            unit = unit ?? TimeQuantity.BaseUnit;
            var quantity = new TimeQuantity(unit.Value, value);
            return quantity;
        }

        /// <summary>
        /// Returns a new <see cref="TimeQuantity"/> in the specified <paramref name="unit"/>
        /// converting from given the <paramref name="quantity"/>.
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static TimeQuantity ToTimeQuantity(this TimeQuantity quantity, TimeUnit? unit = null)
        {
            unit = unit ?? TimeQuantity.BaseUnit;

            if (unit == quantity.Unit)
                return new TimeQuantity(quantity.Unit, quantity.Value);

            // ReSharper disable once UseObjectOrCollectionInitializer
            var result = new TimeQuantity(quantity.Unit, quantity.Value);
            result.Unit = unit.Value;

            return result;
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> corresponding to the <paramref name="quantity"/>.
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this TimeQuantity quantity)
        {
            return TimeSpan.FromMilliseconds(quantity.ToTimeQuantity(TimeUnit.Millisecond).Value);
        }

        /// <summary>
        /// Returns a <see cref="TimeQuantity"/> corresponding to the <paramref name="timeSpan"/> in the desired <paramref name="unit"/>.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static TimeQuantity ToTimeQuantity(this TimeSpan timeSpan, TimeUnit unit = TimeUnit.Millisecond)
        {
            return timeSpan.TotalMilliseconds.ToTimeQuantity(TimeUnit.Millisecond).ToTimeQuantity(unit);
        }
    }
}
