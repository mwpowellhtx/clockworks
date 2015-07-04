using NUnit.Framework;

namespace Kingdom.Clockworks
{
    internal class RequestTypeValuesAttribute : ValuesAttribute
    {
        internal RequestTypeValuesAttribute()
            : base(RequestType.Instantaneous, RequestType.Continuous)
        {
        }
    }
}
