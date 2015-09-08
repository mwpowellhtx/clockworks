namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    using E = Energy;
    using M = Mass;
    using Theta = Temperature;

    /// <summary>
    /// 
    /// </summary>
    public static class Values
    {
        /// <summary>
        /// StandardGravity on Earth.
        /// </summary>
        /// <a href="http://en.wikipedia.org/wiki/Standard_gravity" >Standard gravity</a>
        public static readonly IQuantity G;

        /// <summary>
        /// Gets the Specific Gas Constant for Dry Air, usually denoted as R<sub>specific</sub>.
        /// </summary>
        /// <a href="!:http://en.wikipedia.org/wiki/Gas_constant#Specific_gas_constant" >Gas constant, specific gas constant</a>
        public static readonly IQuantity R;

        /// <summary>
        /// Static Constructor
        /// </summary>
        static Values()
        {
            G = new Quantity(9.80665d, Acceleration.MetersPerSecondSquared);
            // TODO: Specific Gas Constant: potentially deserving of its own dimension: E M^-1 Theta^-1
            R = new Quantity(287.058d, E.Joule, M.Kilogram.Invert(), Theta.Kelvin.Invert());
        }

        /// <summary>
        /// Gets the <see cref="G"/> standard gravity on Earth.
        /// </summary>
        public static IQuantity StandardGravity
        {
            get { return G; }
        }

        /// <summary>
        /// Gets the specific gas constant, also <see cref="R"/>.
        /// </summary>
        public static IQuantity SpecificGasConstant
        {
            get { return R; }
        }

        //TODO: TBD: may be able to calculate air density, humid air density, etc, here? or a specific calculators area for that ...
    }
}
