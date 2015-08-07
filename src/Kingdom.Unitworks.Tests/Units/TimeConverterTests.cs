using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Kingdom.Unitworks.Units
{
    /// <summary>
    /// This is a key fixture. If these tests fail to work correctly then ther rest
    /// of the time components really do not matter that much.
    /// </summary>
    public class TimeConverterTests : TimeUnitTestFixtureBase
    {
        /// <summary>
        /// Runs a <see cref="TimeConverter"/> <paramref name="scenario"/>.
        /// </summary>
        /// <param name="scenario"></param>
        private static void RunConverterScenario(Action<TimeConverter> scenario)
        {
            Assert.That(scenario, Is.Not.Null);
            var converter = new TimeConverter();
            Assert.That(converter, Is.Not.Null);
            scenario(converter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static IEnumerable<T> GetEnumValues<T>()
            where T : struct
        {
            var type = typeof (T);
            Assert.That(type.IsEnum);
            return Enum.GetValues(typeof (T)).OfType<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void There_is_only_one_base_unit()
        {
            var units = GetEnumValues<TimeUnit>().ToArray();

            Assert.That(units.Count(x => x.IsBaseUnit()), Is.EqualTo(1));

            var baseUnit = units.Single(x => x.IsBaseUnit());

            RunConverterScenario(converter =>
            {
                Assert.That(converter.BaseUnit, Is.EqualTo(baseUnit));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fromUnit"></param>
        /// <param name="toUnit"></param>
        /// <param name="baseUnit"></param>
        /// <returns></returns>
        private double GetExpectedConvertResult(double value, TimeUnit fromUnit, TimeUnit toUnit, TimeUnit baseUnit)
        {
            var baseValue = fromUnit == baseUnit ? value : ConvertToBase(fromUnit, value);
            var result = toUnit == baseUnit ? baseValue : ConvertFromBase(toUnit, baseValue);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromUnit"></param>
        /// <param name="fromValue"></param>
        /// <param name="toUnit"></param>
        [Test]
        [Combinatorial]
        public void Converter_converts_value_correctly([TimeUnitValues] TimeUnit fromUnit,
            [TimeQuantityValues] double fromValue, [TimeUnitValues] TimeUnit toUnit)
        {
            RunConverterScenario(converter =>
            {
                var expectedValue = GetExpectedConvertResult(fromValue, fromUnit, toUnit, converter.BaseUnit);

                var actualValue = converter.Convert(fromValue, fromUnit, toUnit);

                Assert.That(actualValue, Is.EqualTo(expectedValue).Within(Epsilon));
            });
        }
    }
}
