using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    using M = Mass;
    using L = Length;

    public class MassDensityTests : DerivedDimensionTestFixtureBase<MassDensity, IMassDensity>
    {
        private readonly string[] _masses = {"Kilogram"};
        private readonly string[] _lengths = {"Meter", "Kilometer"};

        /// <summary>
        /// Returns the calculated name given <paramref name="m"/> and <paramref name="l"/>.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        private static string GetName(string m, string l)
        {
            return m + "PerCubic" + l;
        }

        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get
            {
                return from m in _masses
                    from l in _lengths
                    select new TestCaseData(GetName(m, l));
            }
        }

        private static bool GetBaseUnit(string m, string l)
        {
            return m + l == "KilogramMeter";
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                return from m in _masses
                    from l in _lengths
                    select new TestCaseData(GetName(m, l), GetBaseUnit(m, l));
            }
        }

        private static readonly IDictionary<string, double?> Factors = new Dictionary<string, double?>
        {
            {"Kilogram", null},
            {"Meter", null},
            {"Kilometer", L.MetersPerKilometer},
        };

        private static double CalculateFactor(string m, string l)
        {
            const double one = 1;
            return (Factors[m] ?? one)/(Factors[l] ?? one).Cubed();
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                return from m in _masses
                    from l in _lengths
                    select new TestCaseData(GetName(m, l), value*CalculateFactor(m, l));
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                //TODO: except for kg km^-3, which is simply a ridiculous dimension...
                return from m in _masses
                    from l in _lengths
                    where m + l != "KilogramKilometer"
                    select new TestCaseData(GetName(m, l),
                        value*CalculateFactor(m, l).Inverted());
            }
        }
    }
}
