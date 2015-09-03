namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using SiValues = SI.Values;

    /// <summary>
    /// 
    /// </summary>
    public static class Values
    {
        /// <summary>
        /// Standard gravity on Earth in US units.
        /// </summary>
        /// <a href="http://en.wikipedia.org/wiki/Standard_gravity" >Standard gravity</a>
        public static readonly IQuantity G;

        /// <summary>
        /// 
        /// </summary>
        public static readonly IQuantity Gmph;

        /// <summary>
        /// Static Constructor
        /// </summary>
        static Values()
        {
            G = SiValues.G.ConvertTo(Acceleration.FeetPerSecondSquared);
            Gmph = SiValues.G.ConvertTo(Acceleration.MilesPerHourSquared);
        }

        /// <summary>
        /// Gets the <see cref="G"/> standard gravity on Earth.
        /// </summary>
        public static IQuantity StandardGravity
        {
            get { return G; }
        }
    }
}
