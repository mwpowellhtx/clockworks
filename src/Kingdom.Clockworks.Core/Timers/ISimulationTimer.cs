using System;

namespace Kingdom.Clockworks.Timers
{
    /// <summary>
    /// Represents the sum total of simulation timer interfaces.
    /// </summary>
    public interface ISimulationTimer
        : IScaleableClock
            , ISteppableClock
            , IMeasurableClock<TimerRequest>
            , IStartableClock
            , IDisposable
    {
    }
}
