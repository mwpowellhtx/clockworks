using System;
using System.Collections.Generic;
using System.Linq;
using Kingdom.Unitworks.Dimensions;

namespace Kingdom.Unitworks
{
    using UsTheta = Dimensions.Systems.US.Angle;
    using SiTheta = Dimensions.Systems.SI.Angle;

    /// <summary>
    /// 
    /// </summary>
    /// <a href="!:http://www.uwgb.edu/dutchs/structge/sl01.htm" > Trigonometry Refresher </a>
    /// <a href="!:http://stackoverflow.com/questions/12108651/math-sin-math-cos-and-math-tan-precision-and-way-to-display-them-correctly" />
    /// <a href="!:http://www.math-only-math.com/trigonometrical-ratios-of-270-degree-minus-theta.html" />
    /// <a href="!:http://www.mathopenref.com/triggraphtan.html" />
    /// <a href="!:http://www.mathsrevision.net/advanced-level-maths-revision/pure-maths/trigonometry/sin-cos-and-tan" />
    public static class TrigonometricExtensionMethods
    {
        private static void VerifyNotNull(this IQuantity quantity)
        {
            if (quantity != null) return;

            throw new ArgumentNullException("quantity", "Unable to perform function on null quantity.");
        }

        private static void VerifyAngular(this IQuantity quantity)
        {
            quantity.VerifyNotNull();

            IEnumerable<IDimension[]> expected = new IDimension[][]
            {
                new[] {SiTheta.Radian},
                new[] {Dimensions.Systems.US.Angle.Degree}
            };

            if (expected.Any(x => quantity.Dimensions.AreEquivalent(x)))
                return;

            var message = string.Format("Unable to perfom function on quantity {{{0}}}.", quantity);
            throw new IncompatibleDimensionsException(message, quantity);
        }

        private static void VerifyDimensionless(this IQuantity quantity)
        {
            quantity.VerifyNotNull();

            if (quantity.IsDimensionless) return;

            var message = string.Format("Unable to perfom function on quantity {{{0}}}.", quantity);
            throw new IncompatibleDimensionsException(message, quantity);
        }

        //private static bool IsDegree(this IQuantity angle, double expectedAngle)
        //{
        //    angle.VerifyAngular();
        //    // TODO: might be better if we had an actual IEquatable<IQuantiy> here ...
        //    return angle.ConvertTo(UsTheta.Degree).Value.Equals(expectedAngle);
        //}

        private static IQuantity CalculateDimensionless(this IQuantity angle, Func<double, double> trigonometry)
        {
            angle.VerifyAngular();

            var theta = angle.ConvertTo(SiTheta.Radian);

            var result = new Quantity(trigonometry(theta.Value));

            return result;
        }

        private static IQuantity CalculateAngular(this IQuantity dimensionless, IAngle desiredUnit,
            Func<double, double> trigonometry)
        {
            dimensionless.VerifyDimensionless();

            var radian = SiTheta.Radian;

            desiredUnit = desiredUnit ?? radian;

            IQuantity result = new Quantity(trigonometry(dimensionless.Value), radian);

            if (desiredUnit is SiTheta) return result;

            return result.ConvertTo(desiredUnit);
        }

        private static IQuantity CalculateAngular(IQuantity x, IQuantity y, IAngle desiredUnit,
            Func<double, double, double> trigonometry)
        {
            //TODO: just dimensionless?
            x.VerifyDimensionless();
            y.VerifyDimensionless();

            var radian = SiTheta.Radian;

            desiredUnit = desiredUnit ?? radian;

            IQuantity result = new Quantity(trigonometry(x.Value, y.Value), radian);

            if (desiredUnit is SiTheta) return result;

            return result.ConvertTo(desiredUnit);
        }

        /// <summary>
        /// Returns the sine of the specified <paramref name="angle"/>.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        /// <see cref="SiTheta.Radian"/>
        /// <see cref="UsTheta.Degree"/>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/system.math.sin.aspx" />
        public static IQuantity Sin(this IQuantity angle)
        {
            return CalculateDimensionless(angle, Math.Sin);
        }

        /// <summary>
        /// Returns the cosine of the specified <paramref name="angle"/>.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        /// <see cref="SiTheta.Radian"/>
        /// <see cref="UsTheta.Degree"/>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/system.math.cos.aspx" />
        public static IQuantity Cos(this IQuantity angle)
        {
            return CalculateDimensionless(angle, Math.Cos);
        }

        /// <summary>
        /// Returns the tangent of the specified <paramref name="angle"/>.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        /// <see cref="SiTheta.Radian"/>
        /// <see cref="UsTheta.Degree"/>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/system.math.tan.aspx" />
        /// <a href="!:http://www.quora.com/Is-tan-90%C2%B0-equal-to-infinity" />
        public static IQuantity Tan(this IQuantity angle)
        {
            /* These are a couple of corner cases that the Math library does not take into
             * account, or returned an "undefined" number, which is technically incorrect.
             * http://www.uwgb.edu/dutchs/structge/sl01.htm */

            const double max = 360d;

            angle.VerifyAngular();

            var angleDegreesValue = angle.ConvertTo(UsTheta.Degree).Value;

            if ((angleDegreesValue%max).Equals(90d))
                return new Quantity(double.PositiveInfinity);

            if ((angleDegreesValue%max).Equals(270d))
                return new Quantity(double.NegativeInfinity);

            return CalculateDimensionless(angle, Math.Tan);
        }

        /// <summary>
        /// Returns the hyperbolic cosine of the specified <paramref name="angle"/>.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        /// <see cref="SiTheta.Radian"/>
        /// <see cref="UsTheta.Degree"/>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/system.math.cosh.aspx" />
        public static IQuantity Cosh(this IQuantity angle)
        {
            return CalculateDimensionless(angle, Math.Cosh);
        }

        /// <summary>
        /// Returns the hyperbolic sine of the specified <paramref name="angle"/>.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        /// <see cref="SiTheta.Radian"/>
        /// <see cref="UsTheta.Degree"/>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/system.math.sinh.aspx" />
        public static IQuantity Sinh(this IQuantity angle)
        {
            return CalculateDimensionless(angle, Math.Sinh);
        }

        /// <summary>
        /// Returns the hyperbolic tangent of the specified <paramref name="angle"/>.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        /// <see cref="SiTheta.Radian"/>
        /// <see cref="UsTheta.Degree"/>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/system.math.tanh.aspx" />
        public static IQuantity Tanh(this IQuantity angle)
        {
            return CalculateDimensionless(angle, Math.Tanh);
        }

        /// <summary>
        /// Returns the angle whose cosine is the specified number.
        /// </summary>
        /// <param name="d">A number representing a cosine, where d must be greater
        /// than or equal to -1, but less than or equal to 1.</param>
        /// <param name="desiredUnit"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/system.math.acos.aspx" />
        public static IQuantity Acos(this IQuantity d, IAngle desiredUnit = null)
        {
            return CalculateAngular(d, desiredUnit, Math.Acos);
        }

        /// <summary>
        /// Returns the angle whose sine is the specified number.
        /// </summary>
        /// <param name="d">A number representing a sine, where d must be greater
        /// than or equal to -1, but less than or equal to 1. </param>
        /// <param name="desiredUnit"></param>
        /// <returns></returns>
        /// <see cref="SiTheta.Radian"/>
        /// <see cref="UsTheta.Degree"/>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/system.math.asin.aspx" />
        public static IQuantity Asin(this IQuantity d, IAngle desiredUnit = null)
        {
            return CalculateAngular(d, desiredUnit, Math.Asin);
        }

        /// <summary>
        /// Returns the angle whose tangent is the specified number.
        /// </summary>
        /// <param name="d">A quantity representing a tangent.</param>
        /// <param name="desiredUnit"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/system.math.atan.aspx" />
        public static IQuantity Atan(this IQuantity d, IAngle desiredUnit = null)
        {
            return CalculateAngular(d, desiredUnit, Math.Atan);
        }

        /// <summary>
        /// Returns the angle whose tangent is the quotient of two specified numbers.
        /// </summary>
        /// <param name="x">The x coordinate of a point.</param>
        /// <param name="y">The y coordinate of a point.</param>
        /// <param name="desiredUnit"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/system.math.atan2.aspx" />
        public static IQuantity Atan2(this IQuantity x, IQuantity y, IAngle desiredUnit = null)
        {
            return CalculateAngular(x, y, desiredUnit, Math.Atan2);
        }

        /// <summary>
        /// Returns the secant of the specified <paramref name="angle"/>.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        /// <see cref="SiTheta.Radian"/>
        /// <see cref="UsTheta.Degree"/>
        public static IQuantity Sec(this IQuantity angle)
        {
            angle.VerifyAngular();

            var cosine = (Quantity) CalculateDimensionless(angle, Math.Cos);

            return 1d/cosine;
        }

        /// <summary>
        /// Returns the cosecant of the specified <paramref name="angle"/>.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        /// <see cref="SiTheta.Radian"/>
        /// <see cref="UsTheta.Degree"/>
        public static IQuantity Csc(this IQuantity angle)
        {
            angle.VerifyAngular();

            var sine = (Quantity) CalculateDimensionless(angle, Math.Sin);

            return 1d/sine;
        }

        /// <summary>
        /// Returns the cotangent of the specified <paramref name="angle"/>.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        /// <see cref="SiTheta.Radian"/>
        /// <see cref="UsTheta.Degree"/>
        public static IQuantity Cot(this IQuantity angle)
        {
            const double max = 360d;

            angle.VerifyAngular();

            /* The operators are still a little bit of a work in progress, handling what to do
             * about converting to/from base approaching the core calculation. For purposes of
             * trig functions, however, we actually sometimes want these in terms of degrees,
             * not Radians, for purposes of evaluation. */

            var angleDegreesValue = angle.ConvertTo(UsTheta.Degree).Value;

            // This is likely not wholely correct, but it is close enough.
            if ((angleDegreesValue%max).Equals(0d))
                return new Quantity(double.PositiveInfinity);

            if ((angleDegreesValue%max).Equals(180d))
                return new Quantity(double.NegativeInfinity);

            var sine = CalculateDimensionless(angle, Math.Sin);
            var cosine = (Quantity) CalculateDimensionless(angle, Math.Cos);

            return cosine/sine;
        }
    }
}
