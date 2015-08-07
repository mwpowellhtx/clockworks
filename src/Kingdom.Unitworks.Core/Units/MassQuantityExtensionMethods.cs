namespace Kingdom.Unitworks.Units
{
    /// <summary>
    /// Mass quantity extension methods.
    /// </summary>
    public static class MassQuantityExtensionMethods
    {
        /// <summary>
        /// Returns a new <see cref="MassQuantity"/> corresponding to the <paramref name="unit"/> and <paramref name="value"/>.
        /// The <see cref="MassQuantity.BaseUnit"/> is assumed when unit is Null.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static MassQuantity ToMassQuantity(this double value, MassUnit? unit = null)
        {
            unit = unit ?? MassQuantity.BaseUnit;
            var quantity = new MassQuantity(unit.Value, value);
            return quantity;
        }

        /// <summary>
        /// Returns a new <see cref="MassQuantity"/> in the specified <paramref name="unit"/>
        /// converting from given the <paramref name="quantity"/>.
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static MassQuantity ToMassQuantity(this MassQuantity quantity, MassUnit? unit = null)
        {
            unit = unit ?? MassQuantity.BaseUnit;

            if (unit == quantity.Unit)
                return new MassQuantity(quantity.Unit, quantity.Value);

            // ReSharper disable once UseObjectOrCollectionInitializer
            var result = new MassQuantity(quantity.Unit, quantity.Value);
            result.Unit = unit.Value;

            return result;
        }
    }
}
