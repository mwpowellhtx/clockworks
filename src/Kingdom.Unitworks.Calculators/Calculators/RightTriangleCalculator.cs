using System;
using System.Linq;

namespace Kingdom.Unitworks.Calculators
{
    using L = Dimensions.Systems.SI.Length;
    using Theta = Dimensions.Systems.SI.PlanarAngle;

    /// <summary>
    /// Performs various right triangle calculations.
    /// </summary>
    /// <a href="!:http://en.wikipedia.org/wiki/Pythagorean_theorem" >Pythagorean theorem</a>
    /// <a href="!:http://www.mathportal.org/calculators/plane-geometry-calculators/right-triangle-calculator.php"
    /// >Calculators, plane geometry, right triangle calculator</a>
    public class RightTriangleCalculator : CalculatorBase, IRightTriangleCalculator
    {
        /// <summary>
        /// Returns the calculated Hypotenuse given <paramref name="aQty"/> and <paramref name="bQty"/> lengths.
        /// <see cref="IRightTriangleCalculator.CalculateAngleBeta"/>
        /// </summary>
        /// <param name="aQty"></param>
        /// <param name="bQty"></param>
        /// <returns></returns>
        public IQuantity CalculateHypotenuse(IQuantity aQty, IQuantity bQty)
        {
            var m = L.Meter;

            var a = VerifyDimensions(aQty, m);
            var b = VerifyDimensions(bQty, m);

            var cQty = ((Quantity) a.Squared() + b.Squared()).SquareRoot();

            return VerifyDimensions(cQty, m);
        }

        /// <summary>
        /// Returns the calculated non-hypotenuse side. This means that <paramref name="abQty"/>
        /// represents either <strong>a</strong> or <strong>b</strong> of the sides. The
        /// calculation results in the other, either <strong>b</strong> or <strong>a</strong>,
        /// respectively. It is up to the caller to know <strong>a</strong> from <strong>b</strong>.
        /// Yields <see cref="double.NaN"/> based quantities when <paramref name="abQty"/> squared
        /// is greater than <paramref name="cQty"/> squared. This is because <see cref="Math.Sqrt"/>
        /// of a negative number is not a number.
        /// </summary>
        /// <param name="cQty"></param>
        /// <param name="abQty"></param>
        /// <returns></returns>
        public IQuantity CalculateNonHypotenuse(IQuantity cQty, IQuantity abQty)
        {
            var m = L.Meter;

            var c = VerifyDimensions(cQty, m);
            var ab = VerifyDimensions(abQty, m);

            var baQty = (((Quantity) c.Squared()) - ab.Squared()).SquareRoot();

            return VerifyDimensions(baQty, m);
        }

        /// <summary>
        /// Verifies that the non-null <paramref name="qties"/> are all positive.
        /// </summary>
        /// <param name="qties"></param>
        private static void VerifyPositive(params IQuantity[] qties)
        {
            foreach (var message in (from x in qties
                where ReferenceEquals(null, x) == false
                      && x.Value <= 0d
                select x)
                .Select(qty => string.Format("Expecting non null, positive quantites but was {{{0}}}.", qty)))
            {
                throw new ArgumentException("qties", message);
            }
        }

        /// <summary>
        /// Verifies that there are sufficient sides <paramref name="aQty"/>,
        /// <paramref name="bQty"/>, or <paramref name="cQty"/> provided.
        /// </summary>
        /// <param name="aQty"></param>
        /// <param name="bQty"></param>
        /// <param name="cQty"></param>
        private static void VerifyAngleCalculationTriangleSides(IQuantity aQty, IQuantity bQty, IQuantity cQty)
        {
            const string message = "At least two lengths must be provided.";

            if (AreNull(aQty, bQty, cQty) || AreNull(aQty, bQty) || AreNull(aQty, cQty))
                throw new ArgumentNullException("aQty", message);

            if (AreNull(bQty, cQty))
                throw new ArgumentNullException("bQty", message);

            VerifyPositive(aQty, bQty, cQty);
        }

        /// <summary>
        /// Returns the calculated angle Alpha, α, given any two of <paramref name="aQty"/>,
        /// <paramref name="bQty"/>, or <paramref name="cQty"/>. If all three are provided, then
        /// the first two are selected. If one or none are provided, then an exception is thrown.
        /// </summary>
        /// <param name="aQty"></param>
        /// <param name="bQty"></param>
        /// <param name="cQty"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IQuantity CalculateAngleAlpha(IQuantity aQty = null,
            IQuantity bQty = null, IQuantity cQty = null)
        {
            VerifyAngleCalculationTriangleSides(aQty, bQty, cQty);

            var m = L.Meter;
            IQuantity betaQty = null;

            if (AreNotNull(aQty, bQty))
            {
                var a = VerifyDimensions(aQty, m);
                var b = VerifyDimensions(bQty, m);
                betaQty = ((Quantity) a/b).Atan();
            }

            if (AreNotNull(aQty, cQty))
            {
                var a = VerifyDimensions(aQty, m);
                var c = VerifyDimensions(cQty, m);
                betaQty = ((Quantity) a/c).Asin();
            }

            if (AreNotNull(bQty, cQty))
            {
                var b = VerifyDimensions(bQty, m);
                var c = VerifyDimensions(cQty, m);
                betaQty = ((Quantity) b/c).Acos();
            }

            var rad = Theta.Radian;

            return VerifyDimensions(betaQty, rad);
        }

        /// <summary>
        /// Returns the calculated angle Beta, ß, given any two of <paramref name="aQty"/>,
        /// <paramref name="bQty"/>, or <paramref name="cQty"/>. If all three are provided, then
        /// the first two are selected. If one or none are provided, then an exception is thrown.
        /// </summary>
        /// <param name="aQty"></param>
        /// <param name="bQty"></param>
        /// <param name="cQty"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IQuantity CalculateAngleBeta(IQuantity aQty = null,
            IQuantity bQty = null, IQuantity cQty = null)
        {
            VerifyAngleCalculationTriangleSides(aQty, bQty, cQty);

            var m = L.Meter;
            IQuantity betaQty = null;

            if (AreNotNull(aQty, bQty))
            {
                var a = VerifyDimensions(aQty, m);
                var b = VerifyDimensions(bQty, m);
                betaQty = ((Quantity) b/a).Atan();
            }

            if (AreNotNull(aQty, cQty))
            {
                var a = VerifyDimensions(aQty, m);
                var c = VerifyDimensions(cQty, m);
                betaQty = ((Quantity) a/c).Acos();
            }

            if (AreNotNull(bQty, cQty))
            {
                var b = VerifyDimensions(bQty, m);
                var c = VerifyDimensions(cQty, m);
                betaQty = ((Quantity) b/c).Asin();
            }

            var rad = Theta.Radian;

            return VerifyDimensions(betaQty, rad);
        }

        /// <summary>
        /// Returns whether all the <paramref name="qties"/> are not null.
        /// </summary>
        /// <param name="qties"></param>
        /// <returns></returns>
        private static bool AreNotNull(params IQuantity[] qties)
        {
            return qties.All(x => !ReferenceEquals(null, x));
        }

        /// <summary>
        /// Returns whether all the <paramref name="qties"/> are null.
        /// </summary>
        /// <param name="qties"></param>
        /// <returns></returns>
        private static bool AreNull(params IQuantity[] qties)
        {
            return qties.All(x => ReferenceEquals(null, x));
        }
    }
}
