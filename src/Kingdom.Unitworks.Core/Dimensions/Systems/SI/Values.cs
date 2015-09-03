namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
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
        /// Static Constructor
        /// </summary>
        static Values()
        {
            G = new Quantity(9.80665d, Acceleration.MetersPerSecondSquared);
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
