﻿using System;

namespace Kingdom.Clockworks.Stopwatches
{
    //TODO: may need/want high resolution capability besides DateTime ("low" resolution)...

    /// <summary>
    /// Represents the sum total of simulation stopwatch interfaces.
    /// </summary>
    public interface ISimulationStopwatch
        : IScaleableClock
            , ISteppableClock
            , IMeasurableClock<StopwatchRequest>
            , IStartableClock
            , IDisposable
    {
    }
}
