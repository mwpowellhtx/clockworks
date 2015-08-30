using NUnit.Framework;

namespace Kingdom.Clockworks
{
    internal class BinaryOperatorChangeTypeValuesAttribute : ValuesAttribute
    {
        private static readonly object[] Values;

        static BinaryOperatorChangeTypeValuesAttribute()
        {
            const ChangeType op = ChangeType.Operator;
            Values = new object[] {op | ChangeType.Addition, op | ChangeType.Subtraction};
        }

        public BinaryOperatorChangeTypeValuesAttribute()
            : base(Values)
        {
        }
    }
}
