using System;
using Kingdom.Unitworks.Dimensions;
using Kingdom.Unitworks.Dimensions.Systems.SI;

namespace Kingdom.Unitworks.Calculators
{
    using A = Area;
    using L = Length;

    /// <summary>
    /// Circular calculators handle not only circle calculations but also elliptical calculations.
    /// </summary>
    public class CircularCalculator : CalculatorBase, ICircularCalculator
    {
        /// <summary>
        /// Returns the calculated Radius given the <paramref name="qty"/>.
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <a href="!:http://www.coolmath.com/reference/circles-geometry#The_radius_of_a_circle" >circles geometry (the radius of a circle)</a>
        public IQuantity CalculateRadius(IQuantity qty,
            CircularCalculationType type = CircularCalculationType.Diameter)
        {
            return CalculateRadiusFromArea(qty, type)
                   ?? CalculateRadiusFromDiameter(qty, type)
                   ?? CalculateRadiusFromCircumference(qty, type);
        }

        /// <summary>
        /// Returns the Radius calculated from the given <see cref="IArea"/> expressed through
        /// <paramref name="qty"/>. The resulting units will be in alignment with said Qty.
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <a href="!:http://en.wikipedia.org/wiki/Arc_%28geometry%29" >Area (geometry)</a>
        private static IQuantity CalculateRadiusFromArea(IQuantity qty, CircularCalculationType type)
        {
            if (!type.HasMask(CircularCalculationType.Area)) return null;

            // Dimensions should be Length Squared. Does not matter the unit at this moment.
            qty.VerifyDimensions(L.Meter.Squared());

            var resultQty = ((Quantity) qty/Math.PI).SquareRoot();

            return resultQty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IQuantity CalculateRadiusFromDiameter(IQuantity qty, CircularCalculationType type)
        {
            if (!type.HasMask(CircularCalculationType.Diameter)) return null;

            // The dimension should be consistently Length, although the unit itself may be different.
            qty.VerifyDimensions(L.Meter);

            var resultQty = (Quantity) qty/2d;

            return resultQty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <a href="!:http://en.wikipedia.org/wiki/Circumference" >Circumference</a>
        /// <a href="!:http://en.wikipedia.org/wiki/Pi" >Pi</a>
        private static IQuantity CalculateRadiusFromCircumference(IQuantity qty, CircularCalculationType type)
        {
            if (!type.HasMask(CircularCalculationType.Circumference)) return null;

            //TODO: for these types of calculations, it might make sense to capture a "derived" unit of "circumference", based on "length", but for first class treatment...
            qty.VerifyDimensions(L.Meter);

            var resultQty = (Quantity) qty/(2d*Math.PI);

            return resultQty;
        }

        /// <summary>
        /// Returns the calculated Diameter given the <paramref name="qty"/>.
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IQuantity CalculateDiameter(IQuantity qty,
            CircularCalculationType type = CircularCalculationType.Radius)
        {
            return CalculateDiameterFromArea(qty, type)
                   ?? CalculateDiameterFromRadius(qty, type)
                   ?? CalculateDiameterFromCircumference(qty, type);
        }

        private static IQuantity CalculateDiameterFromRadius(IQuantity qty, CircularCalculationType type)
        {
            if (!type.HasFlag(CircularCalculationType.Radius)) return null;

            qty.VerifyDimensions(L.Meter);

            var resultQty = (Quantity) qty*2d;

            return resultQty;
        }

        /// <summary>
        /// Returns the diameter calculated from a circumference.
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IQuantity CalculateDiameterFromCircumference(IQuantity qty, CircularCalculationType type)
        {
            var radiusQty = CalculateRadiusFromCircumference(qty, type);
            return ReferenceEquals(null, radiusQty)
                ? null
                : CalculateDiameterFromRadius(radiusQty, CircularCalculationType.Radius);
        }

        /// <summary>
        /// Returns the diameter calculated from a area.
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IQuantity CalculateDiameterFromArea(IQuantity qty, CircularCalculationType type)
        {
            var radiusQty = CalculateRadiusFromArea(qty, type);
            return ReferenceEquals(null, radiusQty)
                ? null
                : CalculateDiameterFromRadius(radiusQty, CircularCalculationType.Radius);
        }

        /// <summary>
        /// Returns the calculated Area given the <paramref name="qty"/>.
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IQuantity CalculateArea(IQuantity qty,
            CircularCalculationType type = CircularCalculationType.Diameter)
        {
            return CalculateAreaFromRadius(qty, type)
                   ?? CalculateAreaFromDiameter(qty, type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IQuantity CalculateAreaFromRadius(IQuantity qty, CircularCalculationType type)
        {
            if (!type.HasFlag(CircularCalculationType.Radius)) return null;

            // The dimension should be consistently Length, although the unit itself may be different.
            qty.VerifyDimensions(L.Meter);

            var resultQty = (Quantity) qty.Squared()*Math.PI;

            return resultQty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IQuantity CalculateAreaFromDiameter(IQuantity qty, CircularCalculationType type)
        {
            if (!type.HasFlag(CircularCalculationType.Diameter)) return null;

            // The dimension should be consistently Length, although the unit itself may be different.
            qty.VerifyDimensions(L.Meter);

            var resultQty = (Quantity) ((Quantity) qty/2d).Squared()*Math.PI;

            return resultQty;
        }

        /// <summary>
        /// This is the elliptical form of the same area calculation. Instead of Radius times two,
        /// we have simply <paramref name="aQty"/> times <paramref name="bQty"/>.
        /// </summary>
        /// <param name="aQty"></param>
        /// <param name="bQty"></param>
        /// <returns></returns>
        /// <a href="!:http://www.coolmath.com/reference/circles-geometry#The_area_of_a_circle" >circles geometry (the area of a circle)</a>
        public IQuantity CalculateArea(IQuantity aQty, IQuantity bQty)
        {
            return CalculateEllipticalArea(aQty, bQty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aQty"></param>
        /// <param name="bQty"></param>
        /// <returns></returns>
        /// <a href="http://www.mathsisfun.com/geometry/ellipse.html" >Ellipse</a>
        /// <a href="http://mathworld.wolfram.com/Ellipse.html" >Ellipse</a>
        private static IQuantity CalculateEllipticalArea(IQuantity aQty, IQuantity bQty)
        {
            /* There is no such thing as types here, since by definition elliptical
             * area is calculated from A and B components, or major and minor axes. */

            {
                // The dimensions should be consistently Length, although the unit itself may be different.
                var m = L.Meter;
                aQty.VerifyDimensions(m);
                bQty.VerifyDimensions(m);
            }

            var resultQty = (Quantity) aQty*bQty*Math.PI;

            return resultQty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IQuantity CalculateCircumference(IQuantity qty,
            CircularCalculationType type = CircularCalculationType.Diameter)
        {
            return CalculateCircumferenceFromRadius(qty, type)
                   ?? CalculateCircumferenceFromDiameter(qty, type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IQuantity CalculateCircumferenceFromRadius(IQuantity qty, CircularCalculationType type)
        {
            if (type.HasFlag(CircularCalculationType.Radius)) return null;

            var resultQty = (Quantity) qty*2d*Math.PI;

            return resultQty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IQuantity CalculateCircumferenceFromDiameter(IQuantity qty, CircularCalculationType type)
        {
            if (type.HasFlag(CircularCalculationType.Diameter)) return null;

            /* Results will be in terms of base units. Which is still 2 PI r, but
             * notice the 2 is understood by the Diameter being 2 r to begin with. */

            var resultQty = (Quantity) qty*Math.PI;

            return resultQty;
        }
    }
}
