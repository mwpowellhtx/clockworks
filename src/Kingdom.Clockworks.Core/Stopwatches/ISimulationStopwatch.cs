using Kingdom.Clockworks.Timers;

namespace Kingdom.Clockworks.Stopwatches
{
    //TODO: may need/want high resolution capability besides DateTime ("low" resolution)...
    //TODO: may want to add ability to time "laps" ...

    /// <summary>
    /// Represents the sum total of simulation stopwatch interfaces.
    /// </summary>
    public interface ISimulationStopwatch
        : IClockBase<StopwatchRequest>
    {
    }
}
