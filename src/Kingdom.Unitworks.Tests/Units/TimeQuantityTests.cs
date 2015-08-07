using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Kingdom.Unitworks.Units
{
    /// <summary>
    /// Time quantity test fixture.
    /// </summary>
    public class TimeQuantityTests : TimeUnitTestFixtureBase
    {
        /// <summary>
        /// Tests that the <see cref="TimeQuantity"/> unit converter has been initialized correctly.
        /// </summary>
        [Test]
        public void Check_converter()
        {
            object converter = null;

            TestDelegate statics = () => converter = TimeQuantityFixture.InternalConverter;

            Assert.That(statics, Throws.Nothing);
            Assert.That(converter, Is.Not.Null);
        }

        #region Constructor Members

        /// <summary>
        /// Tests that the default constructor works correctly.
        /// </summary>
        [Test]
        public void Ctor_default()
        {
            new TimeQuantityFixture().Verify<TimeUnit, TimeQuantity>();
        }

        /// <summary>
        /// Tests that the <see cref="TimeUnit"/> constructor works correctly.
        /// </summary>
        /// <param name="unit"></param>
        [Test]
        [Combinatorial]
        public void Ctor_TimeUnit([TimeUnitValues] TimeUnit unit)
        {
            new TimeQuantityFixture(unit).Verify<TimeUnit, TimeQuantity>(unit);
        }

        /// <summary>
        /// Tests that the <see cref="double"/> constructor works correctly.
        /// </summary>
        /// <param name="value"></param>
        [Test]
        [Combinatorial]
        public void Ctor_double([TimeQuantityValues] double value)
        {
            new TimeQuantityFixture(value).Verify<TimeUnit, TimeQuantity>(value: value);
        }

        /// <summary>
        /// Tests that the <see cref="TimeUnit"/> and <see cref="double"/> constructor works correctly.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="value"></param>
        [Test]
        [Combinatorial]
        public void Ctor_TimeUnit_double([TimeUnitValues] TimeUnit unit,
            [TimeQuantityValues] double value)
        {
            new TimeQuantityFixture(unit, value).Verify<TimeUnit, TimeQuantity>(unit, value);
        }

        #endregion

        #region Operator Verification Helpers

        /// <summary>
        /// Verifies that the additive operator works as expected.
        /// </summary>
        /// <param name="part"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static TResult VerifyOperator<TResult>(OperatorPart part, params object[] args)
        {
            return VerifyOperator<TResult>(new[] {part}, args.Select(a => a.GetType()), args);
        }

        /// <summary>
        /// Verifies that the additive operator works as expected.
        /// </summary>
        /// <param name="part"></param>
        /// <param name="argTypes"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static TResult VerifyOperator<TResult>(OperatorPart part, IEnumerable<Type> argTypes,
            params object[] args)
        {
            return VerifyOperator<TResult>(new[] {part}, argTypes, args);
        }

        /// <summary>
        /// Verifies that the additive operator works as expected.
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="types"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static TResult VerifyOperator<TResult>(IEnumerable<OperatorPart> parts,
            IEnumerable<Type> types, params object[] args)
        {
            Assert.That(parts, Is.Not.Null);

            // ReSharper disable PossibleMultipleEnumeration
            {
                return InvokeOperatorAsync<TimeQuantity, TResult>(parts,
                    x => (TResult)Convert.ChangeType(x, typeof(TResult)), types, args)
                    .ContinueWith(task =>
                    {
                        var c = task.Result;

                        Assert.That(args.All(a =>
                        {
                            Assert.That(c, Is.Not.SameAs(a));
                            return true;
                        }));

                        return c;
                    }).Result;
            }
        }

        #endregion

        #region Mathematical Operator Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aUnit"></param>
        /// <param name="aValue"></param>
        /// <param name="bUnit"></param>
        /// <param name="bValue"></param>
        [Test]
        [Combinatorial]
        public void Test_that_addition_operator_works(
            [TimeUnitValues] TimeUnit aUnit, [TimeQuantityValues] double aValue,
            [TimeUnitValues] TimeUnit bUnit, [TimeQuantityValues] double bValue)
        {
            var a = aValue.ToTimeQuantity(aUnit);
            var b = bValue.ToTimeQuantity(bUnit);

            var aBase = ConvertToBase(a.Unit, a.Value);
            var bBase = ConvertToBase(b.Unit, b.Value);

            var expectedValue = ConvertFromBase(aUnit, aBase + bBase);

            VerifyOperator<TimeQuantity>(OperatorPart.Addition, a, b)
                .Verify<TimeUnit, TimeQuantity>(aUnit, expectedValue, Epsilon);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aUnit"></param>
        /// <param name="aValue"></param>
        /// <param name="bUnit"></param>
        /// <param name="bValue"></param>
        [Test]
        [Combinatorial]
        public void Test_that_subtraction_operator_works(
            [TimeUnitValues] TimeUnit aUnit, [TimeQuantityValues] double aValue,
            [TimeUnitValues] TimeUnit bUnit, [TimeQuantityValues] double bValue)
        {
            var a = aValue.ToTimeQuantity(aUnit);
            var b = bValue.ToTimeQuantity(bUnit);

            var aBase = ConvertToBase(a.Unit, a.Value);
            var bBase = ConvertToBase(b.Unit, b.Value);

            var expectedValue = ConvertFromBase(aUnit, aBase - bBase);

            VerifyOperator<TimeQuantity>(OperatorPart.Subtraction, a, b)
                .Verify<TimeUnit, TimeQuantity>(aUnit, expectedValue, Epsilon);
        }

        /// <summary>
        /// Tests that the multiply <see cref="TimeQuantity"/> by <see cref="double"/> operator
        /// works correctly.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="value"></param>
        /// <param name="factor"></param>
        [Test]
        [Combinatorial]
        public void Test_that_multiply_operator_TimeQuantity_double_works(
            [TimeUnitValues] TimeUnit unit, [TimeQuantityValues] double value,
            [TimeQuantityValues] double factor)
        {
            var a = value.ToTimeQuantity(unit);

            var aBase = ConvertToBase(a.Unit, a.Value);

            var expectedValue = ConvertFromBase(unit, aBase*factor);

            VerifyOperator<TimeQuantity>(OperatorPart.Multiply, a, factor)
                .Verify<TimeUnit, TimeQuantity>(unit, expectedValue, Epsilon);
        }

        /// <summary>
        /// Tests that the multiply <see cref="double"/> by <see cref="TimeQuantity"/> operator
        /// works correctly. It is subtle but this is actually a different operator.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="value"></param>
        /// <param name="factor"></param>
        [Test]
        [Combinatorial]
        public void Test_that_multiply_operator_double_TimeQuantity_works(
            [TimeQuantityValues] double factor, [TimeUnitValues] TimeUnit unit,
            [TimeQuantityValues] double value)
        {
            var a = value.ToTimeQuantity(unit);

            var aBase = ConvertToBase(a.Unit, a.Value);

            var expectedValue = ConvertFromBase(unit, factor*aBase);

            VerifyOperator<TimeQuantity>(OperatorPart.Multiply, factor, a)
                .Verify<TimeUnit, TimeQuantity>(unit, expectedValue, Epsilon);
        }

        /// <summary>
        /// Tests that the <see cref="TimeQuantity"/> division operator works correctly.
        /// </summary>
        /// <param name="aUnit"></param>
        /// <param name="aValue"></param>
        /// <param name="bUnit"></param>
        /// <param name="bValue"></param>
        [Test]
        [Combinatorial]
        public void Test_that_division_operator_TimeQuantities_works(
            [TimeUnitValues] TimeUnit aUnit, [TimeQuantityValues] double aValue,
            [TimeUnitValues] TimeUnit bUnit, [TimeQuantityValues] double bValue)
        {
            Assert.That(bValue, Is.Not.EqualTo(0d));

            var a = aValue.ToTimeQuantity(aUnit)
                .Verify<TimeUnit, TimeQuantity>(aUnit, aValue);

            var b = bValue.ToTimeQuantity(bUnit)
                .Verify<TimeUnit, TimeQuantity>(bUnit, bValue);

            var aBase = ConvertToBase(a.Unit, a.Value);
            var bBase = ConvertToBase(b.Unit, b.Value);

            double? expectedResult = null;

            TestDelegate divide = () => expectedResult = aBase/bBase;

            // Remember that the delegate needs to be evaluated before we can verify we have an expectation.
            Assert.That(divide, Throws.Nothing);
            Assert.That(expectedResult.HasValue);

            //TODO: may want to verify DBZ exception here as well...
            // ReSharper disable once PossibleInvalidOperationException
            VerifyOperator<double>(OperatorPart.Division, a, b)
                .Verify(x => Assert.That(x, Is.EqualTo(expectedResult.Value).Within(Epsilon)));
        }

        /// <summary>
        /// Dividing a floating point number by zero yields infinity, which is apparently by design.
        /// See comments and supporting documentation concerning <see cref="TimeQuantity.op_Division"/>.
        /// </summary>
        /// <param name="aUnit"></param>
        /// <param name="aValue"></param>
        /// <param name="bUnit"></param>
        /// <see cref="TimeQuantity.op_Division"/>
        [Test]
        [Combinatorial]
        public void Test_that_division_by_zero_yields_infinity(
            [TimeUnitValues] TimeUnit aUnit, [TimeQuantityValues] double aValue,
            [TimeUnitValues] TimeUnit bUnit)
        {
            var a = aValue.ToTimeQuantity(aUnit);
            var b = 0d.ToTimeQuantity(bUnit);

            VerifyOperator<double>(OperatorPart.Division, a, b)
                .Verify(x => Assert.That(double.IsInfinity(x)));
        }

        #endregion

        #region Relational Operator Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="part"></param>
        /// <param name="expected"></param>
        private static void VerifyRelationalOperator(TimeQuantity a, TimeQuantity b,
            OperatorPart part, bool expected)
        {
            var argTypes = new[] {typeof (TimeQuantity), typeof (TimeQuantity)};
            VerifyOperator<bool>(part, argTypes, a, b)
                .Verify(x => Assert.That(x, Is.EqualTo(expected)));
        }

        /// <summary>
        /// Tests that the relational operators work for Null operands.
        /// </summary>
        /// <param name="part"></param>
        [Test]
        [Combinatorial]
        public void Test_that_relational_operator_Null_operands_works(
            [RelationalOperatorPartValues] OperatorPart part)
        {
            VerifyRelationalOperator(null, null, part,
                new[] {OperatorPart.Equality}.Contains(part));
        }

        /// <summary>
        /// Tests that the relational operators work for one Null operand works.
        /// </summary>
        /// <param name="bUnit"></param>
        /// <param name="bValue"></param>
        /// <param name="part"></param>
        [Test]
        [Combinatorial]
        public void Test_that_relational_operator_left_Null_operand_works(
            [TimeUnitValues] TimeUnit bUnit, [TimeQuantityValues] double bValue,
            [RelationalOperatorPartValues] OperatorPart part)
        {
            VerifyRelationalOperator(null, bValue.ToTimeQuantity(bUnit), part,
                new[] {OperatorPart.Inequality}.Contains(part));
        }

        /// <summary>
        /// Tests that the relational operators work for one Null operand works.
        /// </summary>
        /// <param name="aUnit"></param>
        /// <param name="aValue"></param>
        /// <param name="part"></param>
        [Test]
        [Combinatorial]
        public void Test_that_relational_operator_right_Null_operand_works(
            [TimeUnitValues] TimeUnit aUnit, [TimeQuantityValues] double aValue,
            [RelationalOperatorPartValues] OperatorPart part)
        {
            VerifyRelationalOperator(aValue.ToTimeQuantity(aUnit), null, part,
                new[] {OperatorPart.Inequality}.Contains(part));
        }

        /// <summary>
        /// RelationalOps backing field assumes that base units are involved,
        /// so simply make the comparison once they are given the operands.
        /// </summary>
        private readonly IDictionary<OperatorPart, Func<double, double, bool>> _relationalOps
            = new Dictionary<OperatorPart, Func<double, double, bool>>
            {
                {OperatorPart.Equality, (a, b) => a.Equals(b)},
                {OperatorPart.Inequality, (a, b) => !a.Equals(b)},
                {OperatorPart.LessThan, (a, b) => a < b},
                {OperatorPart.GreaterThan, (a, b) => a > b},
                {OperatorPart.LessThanOrEqual, (a, b) => a <= b},
                {OperatorPart.GreaterThanOrEqual, (a, b) => a >= b},
            };

        /// <summary>
        /// Tests that relational operators for non null operands works correctly.
        /// Expectations are set via the <see cref="_relationalOps"/> backer.
        /// </summary>
        /// <param name="part"></param>
        /// <param name="aUnit"></param>
        /// <param name="aValue"></param>
        /// <param name="bUnit"></param>
        /// <param name="bValue"></param>
        /// <see cref="_relationalOps"/>
        [Test]
        [Combinatorial]
        public void Test_that_relational_operator_NotNull_operands_works(
            [RelationalOperatorPartValues] OperatorPart part,
            [TimeUnitValues] TimeUnit aUnit, [TimeQuantityValues] double aValue,
            [TimeUnitValues] TimeUnit bUnit, [TimeQuantityValues] double bValue)
        {
            Assert.That(_relationalOps.ContainsKey(part));
            var expectedOp = _relationalOps[part];

            var a = aValue.ToTimeQuantity(aUnit);
            var b = bValue.ToTimeQuantity(bUnit);

            var aBase = ConvertToBase(aUnit, aValue);
            var bBase = ConvertToBase(bUnit, bValue);

            VerifyRelationalOperator(a, b, part, expectedOp(aBase, bBase));
        }

        #endregion
    }
}
