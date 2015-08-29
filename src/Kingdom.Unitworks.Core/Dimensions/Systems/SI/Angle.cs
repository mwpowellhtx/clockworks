namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    /// <summary>
    /// 
    /// </summary>
    public class Angle : BaseDimension, IAngle
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly IAngle Radian = new Angle("rad");

        private Angle(string abbreviation)
            : base(abbreviation, null, null)
        {
        }

        private Angle(Angle other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return GetBase<Angle, IAngle>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Angle(this);
        }
    }
}
