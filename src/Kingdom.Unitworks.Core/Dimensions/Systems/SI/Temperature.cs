namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    /// <summary>
    /// 
    /// </summary>
    public class Temperature : BaseDimension, ITemperature
    {
        /// <summary>
        /// Kelvin is a unit of <see cref="ITemperature"/> measure based upon an absolute scale.
        /// </summary>
        /// <a href="!:http://en.wikipedia.org/wiki/Kelvin" >Kelvin scale</a>
        public static readonly ITemperature Kelvin = new Temperature("K");

        private Temperature(string abbreviation)
            : base(abbreviation, null, null)
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
            return GetBase<Temperature, ITemperature>();
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
