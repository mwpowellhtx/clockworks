using System;
using System.Threading;
using Kingdom.Unitworks;
using Kingdom.Unitworks.Dimensions.Systems.Commons;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStartableClock
    {
        /// <summary>
        /// Gets whether the clock IsRunning.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Starts the clock running.
        /// </summary>
        void Start();

        /// <summary>
        /// Starts the clock timer running with the <paramref name="interval"/> in
        /// <see cref="Time.Millisecond"/>. Negative values causes the timer to stop.
        /// <see cref="Timeout.Infinite"/> causes the timer to stop. Only positive
        /// values cause the timer to run.
        /// </summary>
        /// <param name="interval"></param>
        void Start(int interval);

        /// <summary>
        /// Starts the clock timer running with the <paramref name="interval"/> in
        /// <see cref="Time.Millisecond"/>. Negative values causes the timer to stop.
        /// <see cref="Timeout.Infinite"/> causes the timer to stop. Only positive
        /// values cause the timer to run.
        /// </summary>
        /// <param name="interval"></param>
        void Start(long interval);

        /// <summary>
        /// Starts the clock timer running with the <paramref name="interval"/> in
        /// <see cref="Time.Millisecond"/>. Negative values causes the timer to stop.
        /// <see cref="Timeout.Infinite"/> causes the timer to stop. Only positive
        /// values cause the timer to run.
        /// </summary>
        /// <param name="interval"></param>
        void Start(uint interval);

        /// Starts the clock timer running with the <paramref name="interval"/> in
        /// <see cref="Time.Millisecond"/>. Negative values causes the timer to stop.
        /// <see cref="double.NegativeInfinity"/> causes the timer to stop. Only positive
        /// values cause the timer to run.
        void Start(double interval);

        /// <summary>
        /// Starts the clock running using the specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval"></param>
        void Start(TimeSpan interval);

        /// <summary>
        /// Stops the clock from running.
        /// </summary>
        void Stop();
    }

    /// <summary>
    /// Represents an <see cref="IStartableClock"/> with specified <see cref="TimerElapsed"/> event.
    /// </summary>
    /// <typeparam name="TTimerElapsedEventArgs"></typeparam>
    public interface IStartableClock<TTimerElapsedEventArgs> : IStartableClock
        where TTimerElapsedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the internal TimerIntervalQty. Settings the interval to
        /// <see cref="double.NegativeInfinity"/> or <see cref="double.PositiveInfinity"/> stops the
        /// clock timer from running.
        /// </summary>
        IQuantity TimerIntervalQty { get; set; }

        /// <summary>
        /// TimerElapsed event.
        /// </summary>
        event EventHandler<TTimerElapsedEventArgs> TimerElapsed;
    }
}
