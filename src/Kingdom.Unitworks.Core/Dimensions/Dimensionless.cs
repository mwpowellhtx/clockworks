//using Kingdom.Dimensional.Quantities.Dimensions.Systems;

//namespace Kingdom.Unitworks.Dimensions
//{
//    public abstract class Dimensionless : Dimension, IDimensionless
//    {
//        public override bool IsBaseUnit
//        {
//            get { return ToBase.IsIdentity && FromBase.IsIdentity; }
//        }

//        protected Dimensionless(string abbreviation, IUnitConversion toBase, IUnitConversion fromBase)
//            : base(abbreviation, toBase, fromBase)
//        {
//        }

//        protected Dimensionless(Dimensionless other)
//            : base(other)
//        {
//        }

//        public override string ToString()
//        {
//            //TODO: no need to report an exponents here?
//            //TODO: which, should exponents be contained by unary/compound?
//            //TODO: really has no bearing on dimensionless ?
//            var formatted = Exponent == 0
//                ? string.Empty
//                : (Abbreviation + ExponentText);
//            return formatted;
//        }
//    }
//}
