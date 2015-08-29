////TODO: will not worry about this dimension in US units for the time being...
//namespace Kingdom.Dimensional.Quantities.Dimensions.Systems.US
//{
//    using L = Length;
//    using F = Force;

//    public class Energy : EnergyBase
//    {
//        public static readonly IEnergy FootPoundForce = new Energy(L.Foot, F.FootPound);

//        private Energy(ILength length, IForce force)
//            : base(length, force)
//        {
//        }

//        private Energy(Energy other)
//            : base(other)
//        {
//        }

//        public override IDimension GetBase()
//        {
//            return GetBase<SI.Energy, IEnergy>();
//        }

//        public override object Clone()
//        {
//            return new Energy(this);
//        }
//    }
//}
