using System.Diagnostics;
using System.Linq;

namespace Kingdom.Unitworks.Dimensions
{
    using L = Systems.SI.Length;

    /// <summary>
    /// 
    /// </summary>
    /// <a href="!:http://en.wikipedia.org/wiki/Solid_angle" >Solid angle</a>
    public abstract class SolidAngleBase : DerivedDimension, ISolidAngle
    {
        /// <summary>
        /// 
        /// </summary>
        protected IArea SurfaceArea
        {
            get { return Dimensions.OfType<IArea>().SingleOrDefault(); }
        }

        /// <summary>
        /// 
        /// </summary>
        protected ILength SquareRadius
        {
            get { return Dimensions.OfType<ILength>().SingleOrDefault(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <param name="toBase"></param>
        /// <param name="fromBase"></param>
        /// <param name="surfaceArea"></param>
        /// <param name="squareRadius"></param>
        protected SolidAngleBase(string abbreviation,
            IUnitConversion toBase, IUnitConversion fromBase,
            IArea surfaceArea, ILength squareRadius)
            : base(abbreviation, toBase, fromBase, surfaceArea, squareRadius)
        {
            VerifyDimensions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        protected SolidAngleBase(SolidAngleBase other)
            : base(other)
        {
            VerifyDimensions();
        }

        private void VerifyDimensions()
        {
            // Verify that the dimensions are actually dimensionless.
            Debug.Assert(!Dimensions.EnumerateAll().Reduce().Any());

            // ReSharper disable RedundantAssignment
            var surfaceArea = this.SurfaceArea;
            var squareRadius = this.SquareRadius;

            Debug.Assert(surfaceArea != null && surfaceArea.Exponent == 1);
            Debug.Assert(SquareRadius != null && squareRadius.Exponent == -2);

            // Make double sure that Area really is an Area.
            var reducedSurfaceArea = surfaceArea.Dimensions.EnumerateAll().Reduce().ToArray();

            Debug.Assert(reducedSurfaceArea.AreCompatible(new[] {L.Meter.Squared()}));
        }
    }
}
