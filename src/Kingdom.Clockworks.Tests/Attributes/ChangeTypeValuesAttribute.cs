using NUnit.Framework;

namespace Kingdom.Clockworks
{
    internal class InstanceChangeTypeValuesAttribute : ValuesAttribute
    {
        private static readonly object[] Values;

        static InstanceChangeTypeValuesAttribute()
        {
            Values = new object[] {ChangeType.Increment, ChangeType.Decrement};
        }

        public InstanceChangeTypeValuesAttribute()
            : base(Values)
        {
        }
    }
}
