using System.Collections.Generic;
using System.Linq;
using Kingdom.Unitworks;
using NUnit.Framework;

namespace Kingdom.Clockworks
{
    using T = Unitworks.Dimensions.Systems.Commons.Time;

    internal class IntervalTimePerTimeValuesAttribute : ValuesAttribute
    {
        private static readonly object[] Values;

        static IntervalTimePerTimeValuesAttribute()
        {
            var values = new List<IQuantity>();

            var times = new[]
            {
                //TODO: ... however, us, ms, s, min, hr, are not unreasonable to expect ...
                T.Microsecond,
                T.Millisecond,
                T.Second,
                T.Minute,
                T.Hour,
                ////TODO: we do not care about Days or Weeks for purposes of these tests...
                //T.Day,
                //T.Week,
            };

            // TODO: TBD: Avoid scaling too absurdly: may want to be more selective than this...
            var length = times.Length - 2;

            // TODO: TBD: Could also vary the value itself, but this will do for starters...
            const double value = 1e2;

            var first = times.Take(length).ToArray();
            var second = times.Reverse().Take(length).ToArray();

            values.AddRange(from x in first from y in second select new Quantity(value, x, y.Invert()));
            values.AddRange(from y in first from x in second select new Quantity(value, x, y.Invert()));

            Values = values.OfType<object>().ToArray();
        }

        public IntervalTimePerTimeValuesAttribute()
            : base(Values)
        {
        }
    }
}
