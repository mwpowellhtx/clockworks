using System;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISteppableRequest
        : ITimeableRequest
        , IEquatable<ISteppableRequest>
    {
    }
}
