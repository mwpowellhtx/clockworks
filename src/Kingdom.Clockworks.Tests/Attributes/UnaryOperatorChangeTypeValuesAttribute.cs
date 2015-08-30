using NUnit.Framework;

namespace Kingdom.Clockworks
{
    internal class UnaryOperatorChangeTypeValuesAttribute : ValuesAttribute
    {
        private static readonly object[] Values;

        static UnaryOperatorChangeTypeValuesAttribute()
        {
            const ChangeType op = ChangeType.Operator;
            Values = new object[] {op | ChangeType.Increment, op | ChangeType.Decrement};
        }

        public UnaryOperatorChangeTypeValuesAttribute()
            : base(Values)
        {
        }
    }
}
