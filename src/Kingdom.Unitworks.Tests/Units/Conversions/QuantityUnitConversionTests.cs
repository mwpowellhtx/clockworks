using System;
using System.Linq;
using Kingdom.Unitworks.Dimensions;
using NUnit.Framework;

namespace Kingdom.Unitworks.Units.Conversions
{
    using SiL = Dimensions.Systems.SI.Length;
    using SiV = Dimensions.Systems.SI.Velocity;
    using UsL = Dimensions.Systems.US.Length;
    using UsV = Dimensions.Systems.US.Velocity;
    using T = Dimensions.Systems.Commons.Time;

    public class QuantityUnitConversionTests : TestFixtureBase
    {
        /// <summary>
        /// Handles coordinating Conversion from an original dimension unit to a desired
        /// unit. Reports what it did upon disposal.
        /// </summary>
        private class ConversionContext : IDisposable
        {
            private readonly IQuantity _original;

            private IQuantity _converted;

            private static void VerifyUnitsNotSame(IQuantity quantity,
                params IDimension[] units)
            {
                var tupled = units.ToDictionary(u => u.DimensionId,
                    u => new Tuple<IDimension, IDimension>(u,
                        quantity.Dimensions.SingleOrDefault(d => d.DimensionId == u.DimensionId)));

                // The units may indeed be the same dimension but should be different instances.
                foreach (var tuple in tupled.Values.Where(t => t.Item2 != null))
                    Assert.That(tuple.Item1, Is.Not.SameAs(tuple.Item2));
            }

            internal static ConversionContext Create(double value, params IDimension[] units)
            {
                return new ConversionContext(value, units);
            }

            private double _epsilon = 1e-3;

            internal ConversionContext AccurateTo(int decimalPlaces = ThreeDecimalPlaces)
            {
                _epsilon = Math.Pow(10d, -decimalPlaces);
                return this;
            }

            private ConversionContext(double value, params IDimension[] units)
            {
                //Assert.That(units.All(u => u.IsBaseUnit));

                _original = new Quantity(value, units);

                Assert.That(_original.Value, Is.EqualTo(value));

                // TODO: this should be unit tested just besides, on a far smaller scale ...
                VerifyUnitsNotSame(_original, units);
            }

            internal ConversionContext ConvertTo(double expectedValue, params IDimension[] units)
            {
                _converted = _original.ConvertTo(units);

                // We do not care about exponents in this instances, only the dimensions themselves.
                Assert.That(_original.Dimensions.AreCompatible(units, true));

                Assert.That(_converted.Value, Is.EqualTo(expectedValue).Within(_epsilon));

                VerifyUnitsNotSame(_converted, units);

                return this;
            }

            public void Dispose()
            {
                Console.WriteLine("Tested within Epsilon {0}.", _epsilon);

                if (!(_original == null || _converted == null))
                    Console.WriteLine("Compared original {{{0}}} with {{{1}}}.", _original, _converted);
                else if (_converted != null)
                    Console.WriteLine("Unable to compare original {{{0}}}.", _original);
                else
                    Console.WriteLine("Unable to compare original with converted quantity.");
            }
        }

        private const int ThreeDecimalPlaces = 3;
        
        private const int FourDecimalPlaces = 4;

        [Test]
        public void Verify_SI_Length_Meters_Converts_To_US_Miles()
        {
            // http://www.google.com/?gws_rd=ssl#q=meter+to+mile+conversion
            using (ConversionContext.Create(Value, SiL.Meter)
                .AccurateTo(FourDecimalPlaces)
                .ConvertTo(Value/UsL.MetersPerMile, UsL.Mile))
            {
            }
        }

        [Test]
        public void Verify_US_Length_Inches_Converts_To_US_Yards()
        {
            /* Remember it must convert to base unit first then to desired unit, which seems
             * convoluted, but really, it's a trip through multiplication and division equations. */

            const double expectedValue = Value*UsL.MetersPerInch/UsL.MetersPerYard;

            // http://www.google.com/?gws_rd=ssl#q=meter+to+mile+conversion
            using (ConversionContext.Create(Value, UsL.Inch)
                .AccurateTo(FourDecimalPlaces)
                .ConvertTo(expectedValue, UsL.Yard))
            {
            }
        }

        [Test]
        public void Verify_SI_Velocity_Converts_To_US_MilesPerHour()
        {
            const double expectedValue = Value*T.SecondsPerHour/UsL.MetersPerMile;

            using (ConversionContext.Create(Value, SiV.MetersPerSecond)
                .AccurateTo(FourDecimalPlaces)
                .ConvertTo(expectedValue, UsV.MilesPerHour))
            {
            }
        }

        [Test]
        public void Verify_SI_Velocity_Converts_To_US_YardsPerMinute()
        {
            const double expectedValue = Value*T.SecondsPerMinute/UsL.MetersPerYard;

            using (ConversionContext.Create(Value, SiV.MetersPerSecond)
                .AccurateTo(FourDecimalPlaces)
                .ConvertTo(expectedValue, UsV.YardsPerMinute))
            {
            }
        }
    }
}
