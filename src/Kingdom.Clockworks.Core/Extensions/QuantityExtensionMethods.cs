using System;
using System.Linq;
using Kingdom.Unitworks;

namespace Kingdom.Clockworks
{
    using ITime = Unitworks.Dimensions.ITime;
    using T = Unitworks.Dimensions.Systems.Commons.Time;

    //TODO: TBD: consider refactoring to something like Kingdom.Unitworks.Extensions ...

    /// <summary>
    /// 
    /// </summary>
    public static class QuantityExtensionMethods
    {
        private static IQuantity VerifyNotNull(this IQuantity quantity)
        {
            if (quantity == null)
                throw new ArgumentNullException("quantity", "quantity must not be null.");

            return quantity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        private static IQuantity VerifyTimeQuantity(this IQuantity quantity)
        {
            var dimension = quantity.Dimensions.SingleOrDefault();

            // ReSharper disable once InvertIf
            if (!(dimension is ITime) || dimension.GetType() != typeof (T))
            {
                var message = string.Format("quantity dimension must implement {0}", typeof(ITime));
                throw new ArgumentException(message, "quantity");
            }

            return quantity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this IQuantity quantity)
        {
            quantity.VerifyNotNull().VerifyTimeQuantity();

            var dimension = quantity.Dimensions.SingleOrDefault();

            // ReSharper disable once PossibleNullReferenceException
            if (dimension.Equals(T.Microsecond))
            {
                var millisecondQty = quantity.ConvertTo(T.Millisecond);
                millisecondQty.Value = Math.Min(millisecondQty.Value, TimeSpan.MaxValue.TotalMilliseconds - 1);
                millisecondQty.Value = Math.Max(millisecondQty.Value, TimeSpan.MinValue.TotalMilliseconds + 1);
                return TimeSpan.FromMilliseconds(millisecondQty.Value);
            }

            if (dimension.Equals(T.Millisecond))
                return TimeSpan.FromMilliseconds(quantity.Value);

            if (dimension.Equals(T.Second))
                return TimeSpan.FromSeconds(quantity.Value);

            if (dimension.Equals(T.Minute))
                return TimeSpan.FromMinutes(quantity.Value);

            if (dimension.Equals(T.Hour))
                return TimeSpan.FromHours(quantity.Value);

            if (dimension.Equals(T.Day))
                return TimeSpan.FromDays(quantity.Value);

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (dimension.Equals(T.Week))
                return TimeSpan.FromDays(quantity.ConvertTo(T.Day).Value);

            return TimeSpan.Zero;
        }

        /// <summary>
        /// Gets an <see cref="IQuantity"/> given the <see cref="value"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IQuantity ToQuantity(this TimeSpan value)
        {
            return new Quantity(value.TotalMilliseconds, T.Millisecond);
        }
    }
}
