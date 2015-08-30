using System.Collections.Generic;
using System.Linq;
using Kingdom.Unitworks;
using NUnit.Framework;

namespace Kingdom.Clockworks
{
    using T = Unitworks.Dimensions.Systems.Commons.Time;

    internal class TimePerStepValuesAttribute : ValuesAttribute
    {
        private static readonly object[] Values;

        static TimePerStepValuesAttribute()
        {
            var values = new List<IQuantity>();

            var times = new[]
            {
                T.Microsecond,
                T.Millisecond,
                T.Second,
                T.Minute,
                T.Hour,
                //T.Day,
                //T.Week,
            };

            const double value = 1e1;

            values.AddRange(from t in times select new Quantity(value, t));

            Values = values.OfType<object>().ToArray();
        }

        internal TimePerStepValuesAttribute()
            : base(Values)
        {
        }
    }


    //TODO: this one was factored how?
}
