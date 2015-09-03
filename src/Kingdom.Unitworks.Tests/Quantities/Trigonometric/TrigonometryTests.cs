using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Trigonometric
{
    using A = TrigonometryTests.Angles;
    using C = TrigonometryTests.Consts;
    using UsTheta = Dimensions.Systems.US.PlanarAngle;

    /// <summary>
    /// Establishes some trigonometry test cases.
    /// </summary>
    /// <a href="!:http://brownmath.com/twt/refangle.htm" />
    /// <a href="!:http://www.math10.com/en/algebra/sin-cos-tan-cot.html" />
    public class TrigonometryTests : TestFixtureBase
    {
        /// <summary>
        /// This might seem a little extreme, but these are a handful of common angles for which we will test.
        /// </summary>
        internal static class Angles
        {
            internal const double Thirty = 30d;
            internal const double FortyFive = 45d;
            internal const double Sixty = 60d;
            internal const double Nintey = 90d;
            internal const double OneTwenty = 120d;
            internal const double OneThirtyFive = 135d;
            internal const double OneFifty = 150d;
            internal const double OneEighty = 180d;
            internal const double TwoTen = 210d;
            internal const double TwoTwentyFive = 225d;
            internal const double TwoForty = 240d;
            internal const double TwoSeventy = 270d;
            internal const double ThreeHundred = 300d;
            internal const double ThreeFifteen = 315d;
            internal const double ThreeThirty = 330d;
            internal const double ThreeSixty = 360d;
        }

        internal static class Consts
        {
            internal const double Zero = 0d;
            internal const double One = 1d;
            internal const double Two = 2d;
            internal const double Three = 3d;

            internal const double OneHalf = One/Two;

            internal static readonly double SqrtTwo = Math.Sqrt(Two);
            internal static readonly double SqrtThree = Math.Sqrt(Three);

            internal const double PosInf = double.PositiveInfinity;
            internal const double NegInf = double.NegativeInfinity;
        }

        internal static IEnumerable<TestCaseData> SineTestCases
        {
            get
            {
                //TODO: there's a pattern here, but I'm not sure it's worth the effort to describe it, rather than just declare the test cases...
                yield return new TestCaseData(C.Zero, C.Zero);
                yield return new TestCaseData(A.Thirty, C.OneHalf);
                yield return new TestCaseData(A.FortyFive, C.SqrtTwo/C.Two);
                yield return new TestCaseData(A.Sixty, C.SqrtThree/C.Two);
                yield return new TestCaseData(A.Nintey, C.One);
                yield return new TestCaseData(A.OneTwenty, C.SqrtThree/C.Two);
                yield return new TestCaseData(A.OneThirtyFive, C.SqrtTwo/C.Two);
                yield return new TestCaseData(A.OneFifty, C.OneHalf);
                yield return new TestCaseData(A.OneEighty, C.Zero);
                yield return new TestCaseData(A.TwoTen, -C.OneHalf);
                yield return new TestCaseData(A.TwoTwentyFive, -C.SqrtTwo/C.Two);
                yield return new TestCaseData(A.TwoForty, -C.SqrtThree/C.Two);
                yield return new TestCaseData(A.TwoSeventy, -C.One);
                yield return new TestCaseData(A.ThreeHundred, -C.SqrtThree/C.Two);
                yield return new TestCaseData(A.ThreeFifteen, -C.SqrtTwo/C.Two);
                yield return new TestCaseData(A.ThreeThirty, -C.OneHalf);
                yield return new TestCaseData(A.ThreeSixty, C.Zero);
            }
        }

        internal static IEnumerable<TestCaseData> CosineTestCases
        {
            get
            {
                //TODO: there's a pattern here, but I'm not sure it's worth the effort to describe it, rather than just declare the test cases...
                yield return new TestCaseData(C.Zero, C.One);
                yield return new TestCaseData(A.Thirty, C.SqrtThree/C.Two);
                yield return new TestCaseData(A.FortyFive, C.SqrtTwo/C.Two);
                yield return new TestCaseData(A.Sixty, C.OneHalf);
                yield return new TestCaseData(A.Nintey, C.Zero);
                yield return new TestCaseData(A.OneTwenty, -C.OneHalf);
                yield return new TestCaseData(A.OneThirtyFive, -C.SqrtTwo/C.Two);
                yield return new TestCaseData(A.OneFifty, -C.SqrtThree/C.Two);
                yield return new TestCaseData(A.OneEighty, -C.One);
                yield return new TestCaseData(A.TwoTen, -C.SqrtThree/C.Two);
                yield return new TestCaseData(A.TwoTwentyFive, -C.SqrtTwo/C.Two);
                yield return new TestCaseData(A.TwoForty, -C.OneHalf);
                yield return new TestCaseData(A.TwoSeventy, C.Zero);
                yield return new TestCaseData(A.ThreeHundred, C.OneHalf);
                yield return new TestCaseData(A.ThreeFifteen, C.SqrtTwo/C.Two);
                yield return new TestCaseData(A.ThreeThirty, C.SqrtThree/C.Two);
                yield return new TestCaseData(A.ThreeSixty, C.One);
            }
        }

        internal static IEnumerable<TestCaseData> TangentTestCases
        {
            get
            {
                //TODO: there's a pattern here, but I'm not sure it's worth the effort to describe it, rather than just declare the test cases...
                yield return new TestCaseData(C.Zero, C.Zero);
                yield return new TestCaseData(A.Thirty, C.SqrtThree/C.Three);
                yield return new TestCaseData(A.FortyFive, C.One);
                yield return new TestCaseData(A.Sixty, C.SqrtThree);
                yield return new TestCaseData(A.Nintey, C.PosInf);
                yield return new TestCaseData(A.OneTwenty, -C.SqrtThree);
                yield return new TestCaseData(A.OneThirtyFive, -C.One);
                yield return new TestCaseData(A.OneFifty, -C.SqrtThree/C.Three);
                yield return new TestCaseData(A.OneEighty, C.Zero);
                yield return new TestCaseData(A.TwoTen, C.SqrtThree/C.Three);
                yield return new TestCaseData(A.TwoTwentyFive, C.One);
                yield return new TestCaseData(A.TwoForty, C.SqrtThree);
                yield return new TestCaseData(A.TwoSeventy, C.NegInf);
                yield return new TestCaseData(A.ThreeHundred, -C.SqrtThree);
                yield return new TestCaseData(A.ThreeFifteen, -C.One);
                yield return new TestCaseData(A.ThreeThirty, -C.SqrtThree/C.Three);
                yield return new TestCaseData(A.ThreeSixty, C.Zero);
            }
        }

        internal static IEnumerable<TestCaseData> CotangentTestCases
        {
            get
            {
                //TODO: there's a pattern here, but I'm not sure it's worth the effort to describe it, rather than just declare the test cases...
                yield return new TestCaseData(C.Zero, C.PosInf);
                yield return new TestCaseData(A.Thirty, C.SqrtThree);
                yield return new TestCaseData(A.FortyFive, C.One);
                yield return new TestCaseData(A.Sixty, C.SqrtThree/C.Three);
                yield return new TestCaseData(A.Nintey, C.Zero);
                yield return new TestCaseData(A.OneTwenty, -C.SqrtThree/C.Three);
                yield return new TestCaseData(A.OneThirtyFive, -C.One);
                yield return new TestCaseData(A.OneFifty, -C.SqrtThree);
                yield return new TestCaseData(A.OneEighty, C.NegInf);
                yield return new TestCaseData(A.TwoTen, C.SqrtThree);
                yield return new TestCaseData(A.TwoTwentyFive, C.One);
                yield return new TestCaseData(A.TwoForty, C.SqrtThree/C.Three);
                yield return new TestCaseData(A.TwoSeventy, C.Zero);
                yield return new TestCaseData(A.ThreeHundred, -C.SqrtThree/C.Three);
                yield return new TestCaseData(A.ThreeFifteen, -C.One);
                yield return new TestCaseData(A.ThreeThirty, -C.SqrtThree);
                yield return new TestCaseData(A.ThreeSixty, C.PosInf);
            }
        }

        private const double Epsilon = 3e-4;

        private static class TestCases
        {
            /// <summary>
            /// "SineTestCases"
            /// </summary>
            internal const string Sine = "SineTestCases";

            /// <summary>
            /// "CosineTestCases"
            /// </summary>
            internal const string Cosine = "CosineTestCases";

            /// <summary>
            /// "TangentTestCases"
            /// </summary>
            internal const string Tangent = "TangentTestCases";

            /// <summary>
            /// "CotangentTestCases"
            /// </summary>
            internal const string Cotangent = "CotangentTestCases";
        }

        [Test]
        [TestCaseSource(TestCases.Sine)]
        public void Verify_Sine_US_Angle_Degrees(double value, double expectedValue)
        {
            using (new TrigonometryContext()
                .Starting(ctx => new TrigonometricPart(ctx, value,
                    (x, d) => new Quantity(x, d), UsTheta.Degree))
                .Expected(ctx => new TrigonometricPart(ctx, expectedValue, (x, d) => new Quantity(x, d)))
                .SetEpsilon(Epsilon)
                .Function(s => s.Sin()))
            {
            }
        }

        [Test]
        [TestCaseSource(TestCases.Cosine)]
        public void Verify_Cosine_US_Angle_Degrees(double value, double expectedValue)
        {
            using (new TrigonometryContext()
                .Starting(ctx => new TrigonometricPart(ctx, value,
                    (x, d) => new Quantity(x, d), UsTheta.Degree))
                .Expected(ctx => new TrigonometricPart(ctx, expectedValue, (x, d) => new Quantity(x, d)))
                .SetEpsilon(Epsilon)
                .Function(s => s.Cos()))
            {
            }
        }

        [Test]
        [TestCaseSource(TestCases.Tangent)]
        public void Verify_Tangent_US_Angle_Degrees(double value, double expectedValue)
        {
            using (new TrigonometryContext()
                .Starting(ctx => new TrigonometricPart(ctx, value,
                    (x, d) => new Quantity(x, d), UsTheta.Degree))
                .Expected(ctx => new TrigonometricPart(ctx, expectedValue, (x, d) => new Quantity(x, d)))
                .SetEpsilon(Epsilon)
                .Function(s => s.Tan()))
            {
            }
        }

        [Test]
        [TestCaseSource(TestCases.Cotangent)]
        public void Verify_Cotangent_US_Angle_Degrees(double value, double expectedValue)
        {
            using (new TrigonometryContext()
                .Starting(ctx => new TrigonometricPart(ctx, value,
                    (x, d) => new Quantity(x, d), UsTheta.Degree))
                .Expected(ctx => new TrigonometricPart(ctx, expectedValue, (x, d) => new Quantity(x, d)))
                .SetEpsilon(Epsilon)
                .Function(s => s.Cot()))
            {
            }
        }

        ////TODO: atan2 is a special use case: treat it accordingly in the atan functionality...
        //[Ignore("Override to enable for specific unit tests.")]
        //public void Verify_Atan2_Function(double xComponent, double yComponent, double expectedResult)
        //{
        //}
    }
}
