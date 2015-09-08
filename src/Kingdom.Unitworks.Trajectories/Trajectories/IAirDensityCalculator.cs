namespace Kingdom.Unitworks.Trajectories
{
    /// <summary>
    /// Represents an air density calculator.
    /// </summary>
    /// <a href="!:http://en.wikipedia.org/wiki/Density_of_air" >Density of air</a>
    public interface IAirDensityCalculator
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
        IQuantity Calculate(IQuantity absolutePressureQty,
            IQuantity dryAirSpecificGasConstantQty,
            IQuantity temperatureQty);
    }
}
