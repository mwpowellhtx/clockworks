namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    using F = Force;
    using L = Length;

    /// <summary>
    /// 
    /// </summary>
    public class Energy : EnergyBase
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly IEnergy Joule = new Energy("J", F.Newton, L.Meter);

        private Energy(string abbreviation, IForce force, ILength length)
            : base(abbreviation, force, length)
        {
        }

        private Energy(Energy other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return GetBase<Energy, IEnergy>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Energy(this);
        }
    }
}
