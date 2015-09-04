using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.CGS
{
    using M = Mass;
    using L = Length;

    public class MassDensityTests : DerivedDimensionTestFixtureBase<MassDensity, IMassDensity>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get
            {
                yield return new TestCaseData("GramPerCubicCentimeter");
            }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool notBaseUnit = false;

                yield return new TestCaseData("GramPerCubicCentimeter", notBaseUnit);
            }
        }

        private static double CalculateFactor(double m, double l)
        {
            return m/l.Cubed();
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("GramPerCubicCentimeter",
                    value*CalculateFactor(M.KilogramPerGram, L.MetersPerCentimeter));
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("GramPerCubicCentimeter",
                    value*CalculateFactor(M.KilogramPerGram, L.MetersPerCentimeter).Inverted());
            }
        }
    }
}
