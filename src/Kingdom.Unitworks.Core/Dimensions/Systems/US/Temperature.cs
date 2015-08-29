namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    /// <summary>
    /// 
    /// </summary>
    public class Temperature : BaseDimension, ITemperature
    {
        /// <summary>
        /// 
        /// </summary>
        internal static readonly IUnitConversion FahrenheitToBase
            = new BaseDimensionUnitConversion(1d/1.8d, 273.15d, 32d);

        /// <summary>
        /// 
        /// </summary>
        internal static readonly IUnitConversion BaseToFahrenheit
            = new BaseDimensionUnitConversion(1.8d, 32d, -273.15d);

        /// <summary>
        /// 
        /// </summary>
        /// <a href="!:http://www.metric-conversions.org/temperature/kelvin-to-fahrenheit.htm" />
        /// <a href="!:http://www.rapidtables.com/convert/temperature/fahrenheit-to-kelvin.htm" />
        public static readonly ITemperature Fahrenheit = new Temperature("°F",
            FahrenheitToBase, BaseToFahrenheit);

        private Temperature(string abbreviation, IUnitConversion toBase, IUnitConversion fromBase)
            : base(abbreviation, toBase, fromBase)
        {
        }

        private Temperature(Temperature other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return GetBase<SI.Temperature, ITemperature>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Temperature(this);
        }
    }
}
