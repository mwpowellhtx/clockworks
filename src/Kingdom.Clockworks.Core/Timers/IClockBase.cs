using System;

namespace Kingdom.Clockworks.Timers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public interface IClockBase<out TRequest>
        : IScaleableClock
            , ISteppableClock
            , IMeasurableClock<TRequest>
            , IStartableClock
            , IDisposable
        where TRequest : TimeableRequestBase
    {
    }
}
