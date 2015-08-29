using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems
{
    public abstract class DimensionTestFixtureBase<TDimension, TInterface>
        : TestFixtureBase
        where TDimension : class, TInterface
        where TInterface : IDimension
    {
        private static readonly Type InterfaceType = typeof (TInterface);
        private static readonly Type DimensionType = typeof (TDimension);

        ////TODO: it would not be difficult to specify this as well, but I do not expect it is much of an issue for us...
        //private static readonly Type BaseUnitType = typeof(TBaseUnit);

        protected static IEnumerable<TInterface> GetAvailableUnits(Func<FieldInfo, bool> conditions = null)
        {
            conditions = conditions ?? (x => true);

            const BindingFlags flags =
                BindingFlags.Public
                | BindingFlags.Static
                | BindingFlags.GetField
                | BindingFlags.DeclaredOnly;

            // This is the handshake that verifies we've got a dimension defined of the expected type.
            var dimensions = (from f
                in DimensionType.GetFields(flags)
                where f.FieldType == InterfaceType && conditions(f)
                select f.GetValue(null))
                .OfType<TInterface>().ToArray();

            //TODO: fill me in...
            return dimensions;
        }

        protected static TInterface GetUnit(string unitName)
        {
            var unit = GetAvailableUnits(f => f.Name.Equals(unitName)).SingleOrDefault();
            Assert.That(unit, Is.Not.Null);
            return unit;
        }

        protected virtual IEnumerable<TestCaseData> GetDimensionTestCases()
        {
            //TODO: fill me in...
            yield break;
        }

        ////TODO: is no longer done by BaseUnit or here via Reflection
        //protected static TInterface GetDimensionBaseUnit()
        //{
        //    const BindingFlags flags
        //        = BindingFlags.Public
        //          | BindingFlags.Static
        //          | BindingFlags.GetProperty
        //          | BindingFlags.DeclaredOnly;

        //    var propertyInfo = DimensionType.GetProperty("BaseUnit", flags);
        //    Assert.That(propertyInfo, Is.Not.Null);

        //    var propertyValue = propertyInfo.GetValue(null);
        //    Assert.That(propertyValue, Is.Not.Null);

        //    //TODO: not expecting this to be a tremendous issue to contend with, want to test, etc, but I've been wrong before...
        //    /* Note that we just care about the interface type, since the concrete type,
        //     * although canonically the same name, is different. */
        //    Assert.That(propertyValue, Is.InstanceOf<TInterface>());

        //    // Finally do the necessary gymnastics in order to return a fully vetted base unit.
        //    var baseUnit = default(TInterface);
        //    Assert.That(baseUnit, Is.Null);

        //    Func<TInterface> getter = () => (TInterface) propertyValue;
        //    TestDelegate delegatedGetter = () => baseUnit = getter();

        //    Assert.That(delegatedGetter, Throws.Nothing);
        //    Assert.That(baseUnit, Is.Not.Null);

        //    return baseUnit;
        //}

        ////TODO: test obviated by the fact that this is no longer static.
        //[Test]
        //public void Verify_dimension_has_a_base_unit()
        //{
        //    GetDimensionBaseUnit();
        //}

        private static class TestCases
        {
            /// <summary>
            /// "InstanceNamesTestCases"
            /// </summary>
            internal const string InstanceNamesTestCases = "InstanceNamesTestCases";

            /// <summary>
            /// "BaseUnitTestCases"
            /// </summary>
            internal const string BaseUnitTestCases = "BaseUnitTestCases";

            /// <summary>
            /// "ToBaseTestCases"
            /// </summary>
            internal const string ToBaseTestCases = "ToBaseTestCases";

            /// <summary>
            /// "FromBaseTestCases"
            /// </summary>
            internal const string FromBaseTestCases = "FromBaseTestCases";
        }

        protected const double BaseConversionStartValue = 9999d;

        private const double Epsilon = 1e-3;

        protected abstract IEnumerable<TestCaseData> InstanceNamesTestCases { get; }
        protected abstract IEnumerable<TestCaseData> BaseUnitTestCases { get; }
        protected abstract IEnumerable<TestCaseData> ToBaseTestCases { get; }
        protected abstract IEnumerable<TestCaseData> FromBaseTestCases { get; }

        [Test]
        [TestCaseSource(TestCases.InstanceNamesTestCases)]
        public void Verify_dimension_is_correctly_registered(string unitName)
        {
            var unit = GetUnit(unitName);

            Assert.That(unit.DimensionType, Is.Not.Null);
            Assert.That(unit.DimensionType, Is.EqualTo(InterfaceType));

            Assert.That(unit.DimensionId, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        [TestCaseSource(TestCases.InstanceNamesTestCases)]
        public void Verify_unit_base_same_as_dimension_base_unit(string unitName)
        {
            //TODO: could probably test for "has base unit" ; and/or being somewhat consistent with itself
            var unit = GetUnit(unitName);
            //var dimensionBaseUnit = GetDimensionBaseUnit();

            var unitBase = unit.GetBase();

            Assert.That(unitBase, Is.Not.Null);
            //Assert.That(unitBase, Is.SameAs(dimensionBaseUnit));
        }

        [Test]
        [TestCaseSource(TestCases.InstanceNamesTestCases)]
        public void Verify_unit_display_name(string unitName)
        {
            var unit = GetUnit(unitName);

            Assert.That(unit.DisplayName, Is.EqualTo(unitName));
        }

        [Test]
        [TestCaseSource(TestCases.InstanceNamesTestCases)]
        public void Verify_unit_is_instance_of(string unitName)
        {
            var unit = GetUnit(unitName);

            Assert.That(unit, Is.InstanceOf<TDimension>(),
                "Unit {0} is not an instance of {1}.", unitName, DimensionType);

            Assert.That(unit, Is.InstanceOf<TInterface>(),
                "Unit {0} is not an instance of {1}.", unitName, InterfaceType);
        }

        /// <summary>
        /// Verifies whether 
        /// </summary>
        /// <param name="unitName"></param>
        /// <param name="baseUnit"></param>
        [Test]
        [TestCaseSource(TestCases.BaseUnitTestCases)]
        public void Verify_whether_unit_is_base_unit(string unitName, bool baseUnit)
        {
            var unit = GetUnit(unitName);

            //// For whatever reason it seems the assembly under test got "stuck" in mid queue.
            //// Not sure what happened there.
            //if (dimension is IDerivedDimension)
            //{
            //    var dd = dimension as IDerivedDimension;
            //    CollectionAssert.IsNotEmpty(dd.Dimensions);
            //}

            Assert.That(unit.IsBaseUnit, Is.EqualTo(baseUnit));
        }

        [Test]
        [TestCaseSource(TestCases.ToBaseTestCases)]
        public void Verify_unit_to_base(string unitName, double result)
        {
            var unit = GetUnit(unitName);
            VerifyUnitConversion(unit.ToBase, unit.Exponent, result);
        }

        [Test]
        [TestCaseSource(TestCases.FromBaseTestCases)]
        public void Verify_unit_from_base(string unitName, double result)
        {
            var unit = GetUnit(unitName);
            VerifyUnitConversion(unit.FromBase, unit.Exponent, result);
        }

        private static void VerifyUnitConversion(IUnitConversion conversion, int exponent, double result)
        {
            Assert.That(conversion, Is.Not.Null);
            var actual = conversion.Convert(BaseConversionStartValue, exponent);
            Assert.That(actual, Is.EqualTo(result).Within(Epsilon));
        }
    }
}
