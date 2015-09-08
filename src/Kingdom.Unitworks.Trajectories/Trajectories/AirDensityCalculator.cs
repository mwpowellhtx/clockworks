using Kingdom.Unitworks.Calculators;

namespace Kingdom.Unitworks.Trajectories
{
    using P = Dimensions.Systems.SI.Pressure;
    using E = Dimensions.Systems.SI.Energy;
    using M = Dimensions.Systems.SI.Mass;
    using Theta = Dimensions.Systems.SI.Temperature;
    using L = Dimensions.Systems.SI.Length;

    /// <summary>
    /// Represents an air density calculator.
    /// </summary>
    /// <a href="!:http://en.wikipedia.org/wiki/Density_of_air" >Density of air</a>
    public class AirDensityCalculator
        : CalculatorBase
            , IAirDensityCalculator
    {
        /// <summary>
        /// calculates air density, ρ (rho), given <paramref name="absolutePressureQty">absolute pressure,
        /// p</paramref>, <paramref name="dryAirSpecificGasConstantQty">specific gas constant for dry air,
        /// R<sub>specific</sub></paramref>, and <paramref name="temperatureQty">absolute temperature,
        /// K</paramref>.
        /// </summary>
        /// <param name="absolutePressureQty"></param>
        /// <param name="dryAirSpecificGasConstantQty"></param>
        /// <param name="temperatureQty"></param>
        /// <returns></returns>
        /// <a href="!:http://en.wikipedia.org/wiki/Density_of_air#Temperature_and_pressure" >Density of air,
        /// Temperature and pressure</a>
        public IQuantity Calculate(IQuantity absolutePressureQty,
            IQuantity dryAirSpecificGasConstantQty,
            IQuantity temperatureQty)
        {
            var kg = M.Kilogram;
            // ReSharper disable InconsistentNaming
            var K = Theta.Kelvin;

            var p = VerifyDimensions(absolutePressureQty, P.Pascal);

            var Rspecific = VerifyDimensions(dryAirSpecificGasConstantQty, E.Joule, kg.Invert(), K.Invert());

            var T = VerifyDimensions(temperatureQty, K);

            return VerifyDimensions((Quantity) p/((Quantity) Rspecific*T), kg, L.Meter.Cubed().Invert());
        }
    }
}
