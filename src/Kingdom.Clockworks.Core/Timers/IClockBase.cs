using System;

namespace Kingdom.Clockworks.Timers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IClockBase
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public interface IClockBase<out TRequest>
        : IClockBase
            , IScaleableClock
            , ISteppableClock
            , IMeasurableClock<TRequest>
            , IStartableClock
            , IDisposable
        where TRequest : TimeableRequestBase
    {
    }
}
