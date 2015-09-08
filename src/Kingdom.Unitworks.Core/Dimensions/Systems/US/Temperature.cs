namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using SiTheta = SI.Temperature;

    /// <summary>
    /// Represents U.S. system of units of <see cref="ITemperature"/> measure.
    /// </summary>
    public class Temperature : BaseDimension, ITemperature
    {
        /// <summary>
        /// Represents the complex base to Fahrenheit unit calculation. This one is tricky
        /// because there is an inner and an outer offset that must be applied in either
        /// direction to or from base units.
        /// </summary>
        internal static readonly IUnitConversion FahrenheitToBase
            = new BaseDimensionUnitConversion(1d/1.8d, 273.15d, 32d);

        /// <summary>
        /// Represents the complex base to Fahrenheit unit calculation. This one is tricky
        /// because there is an inner and an outer offset that must be applied in either
        /// direction to or from base units.
        /// </summary>
        internal static readonly IUnitConversion BaseToFahrenheit
            = new BaseDimensionUnitConversion(1.8d, 32d, -273.15d);

        /// <summary>
        /// KelvinPerRankine: 5/9
        /// </summary>
        /// <a href="!:http://en.wikipedia.org/wiki/Rankine_scale" >Rankine scale, see &quot;Rankine temperature conversion formulae&quot;</a>
        internal const double KelvinPerRankine = 5d/9d;
        internal const double RankinePerKelvin = 9d/5d;

        /// <summary>
        /// Fahrenheit is not considered an absolute temperature scale. Instead, consider using
        /// <see cref="Rankine"/> for absolute temperature calculations.
        /// </summary>
        /// <a href="!:http://www.metric-conversions.org/temperature/kelvin-to-fahrenheit.htm" />
        /// <a href="!:http://www.rapidtables.com/convert/temperature/fahrenheit-to-kelvin.htm" />
        public static readonly ITemperature Fahrenheit = new Temperature("°F",
            FahrenheitToBase, BaseToFahrenheit);

        /// <summary>
        /// Rankine unit of <see cref="ITemperature"/> measure. Rankine is considered an absolute
        /// unit of measure related to the <see cref="Fahrenheit"/> unit of measure.
        /// </summary>
        /// <a href="!:http://en.wikipedia.org/wiki/Rankine_scale" >Rankine scale</a>
        /// <a href="!:http://en.wikipedia.org/wiki/Absolute_temperature_scale" >Absolute temperature scale</a>
        public static readonly ITemperature Rankine = new Temperature("°R",
            new BaseDimensionUnitConversion(KelvinPerRankine),
            new BaseDimensionUnitConversion(RankinePerKelvin));

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
            return GetBase<SiTheta, ITemperature>();
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
