using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using M = Mass;
    using L = Length;

    public class MassDensityTests : DerivedDimensionTestFixtureBase<MassDensity, IMassDensity>
    {
        private readonly string[] _masses = {"Ounce", "Pound", "Stone"};
        private readonly string[] _lengths = {"Inch", "Foot", "Yard", "Mile"};

        /// <summary>
        /// Returns a calculated name given <paramref name="m"/> and <paramref name="l"/>.
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

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool notBaseUnit = false;

                return from m in _masses
                    from l in _lengths
                    select new TestCaseData(GetName(m, l), notBaseUnit);
            }
        }

        /// <summary>
        /// Represents the common factors for both <see cref="IMass"/> and <see cref="ILength"/>.
        /// </summary>
        private static readonly IDictionary<string, double> Factors = new Dictionary<string, double>
        {
            {"Inch", L.MetersPerInch},
            {"Foot", L.MetersPerFoot},
            {"Yard", L.MetersPerYard},
            {"Mile", L.MetersPerMile},
            {"Ounce", M.KilogramsPerOunce},
            {"Pound", M.KilogramsPerPound},
            {"Stone", M.KilogramsPerStone}
        };

        private static double CalculateFactor(string m, string l)
        {
            return Factors[m]/Factors[l].Cubed();
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                return from m in _masses
                    from l in _lengths
                    select new TestCaseData(GetName(m, l),
                        value*CalculateFactor(m, l));
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                // TODO: Measures everything but st mi^-3, for now, simply ridiculous.
                return from m in _masses
                    from l in _lengths
                    where m + l != "StoneMile"
                    select new TestCaseData(GetName(m, l),
                        value*CalculateFactor(m, l).Inverted());
            }
        }
    }
}