using System.Diagnostics;
using System.Linq;

namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class EnergyBase : DerivedDimension, IEnergy
    {
        ///
        //TODO: Force-Length is one expression: could also be a Pressure-CubicVolume (i.e. lengths cancel out...).
        protected IForce Force
        {
            get { return Dimensions.OfType<IForce>().SingleOrDefault(); }
        }

        /// <summary>
        /// 
        /// </summary>
        protected ILength Length
        {
            get { return Dimensions.OfType<ILength>().SingleOrDefault(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <param name="toBase"></param>
        /// <param name="fromBase"></param>
        protected EnergyBase(string abbreviation, IUnitConversion toBase, IUnitConversion fromBase)
            : base(abbreviation, toBase, fromBase)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <param name="force"></param>
        /// <param name="length"></param>
        protected EnergyBase(string abbreviation, IForce force, ILength length)
            : base(abbreviation, force, length)
        {
            VerifyDimensions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        protected EnergyBase(EnergyBase other)
            : base(other)
        {
            VerifyDimensions();
        }

        private void VerifyDimensions()
        {
            var f = this.Force;
            var l = this.Length;

            Debug.Assert(f != null && f.Exponent == 1);
            Debug.Assert(l != null && l.Exponent == 1);
        }
    }
}
