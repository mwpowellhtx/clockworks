using NUnit.Framework;

namespace Kingdom.Clockworks
{
    internal class TimePerStepValuesAttribute : ValuesAttribute
    {
        internal TimePerStepValuesAttribute()
            : base(50d, 75d, 100d, 125d)
        {
        }
    }


    //TODO: this one was factored how?
    internal class TimeQuantityValuesAttribute : ValuesAttribute
    {
        internal TimeQuantityValuesAttribute()
            : base(100d, 200d, 300d)
        {
        }
    }
}
