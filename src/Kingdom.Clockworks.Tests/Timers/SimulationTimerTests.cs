using Kingdom.Unitworks.Units;

namespace Kingdom.Clockworks.Timers
{
    /// <summary>
    /// Extends the <see cref="TimeableClockTestFixtureBase{TClock, TRequest, TElapsedEventArgs}"/>
    /// to support the <see cref="SimulationTimer"/> and supporting cast of types.
    /// </summary>
    public class SimulationTimerTests
        : TimeableClockTestFixtureBase<
            SimulationTimer
            , TimerRequest
            , TimerElapsedEventArgs>
    {
        /// <summary>
        /// Returns a created <see cref="TimerRequest"/> given the arguments.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="millisecondsPerStep"></param>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected override TimerRequest MakeRequest(RunningDirection? direction = null,
            double millisecondsPerStep = OneSecondMilliseconds, int steps = One,
            RequestType type = RequestType.Instantaneous)
        {
            return new TimerRequest(direction, millisecondsPerStep, steps, type);
        }

        /// <summary>
        /// Performs the arrangement and actions supporting the
        /// <see cref="TimeableClockTestFixtureBase{SimulationTimer, TimerRequest, TimerElapsedEventArgs}.Elapsed_events_yield_expected_event_counts"/>
        /// tests.
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="durationSeconds"></param>
        protected override void MakeElapsedEventYieldArrangements(SimulationTimer timer, double durationSeconds)
        {
            // Should be well within whatever duration we were given.
            timer.SecondsPerSecond = 1/10d;

            // Reset the durations.
            timer.Reset(durationSeconds);
        }

        #region Increment and Decrement Members

        /// <summary>
        /// Initializes <paramref name="theTimer"/> given the arguments.
        /// </summary>
        /// <param name="theTimer"></param>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <param name="millisecondsPerStep"></param>
        protected override void InitializeStarNcrementClock(SimulationTimer theTimer,
            double intervalSecondsPerSecond, double millisecondsPerStep)
        {
            base.InitializeStarNcrementClock(theTimer, intervalSecondsPerSecond,
                millisecondsPerStep);

            // Remember to reset in terms of milliseconds.
            var intervalQuantity = intervalSecondsPerSecond.ToTimeQuantity()
                .ToTimeQuantity(TimeUnit.Millisecond);

            theTimer.TargetMilliseconds = (intervalQuantity*10d).Value;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="e"></param>
        ///// <param name="expectedElapsedMilliseconds"></param>
        //protected override void VerifyElapsedEventResults(TimerElapsedEventArgs e, double expectedElapsedMilliseconds)
        //{
        //    base.VerifyElapsedEventResults(e, expectedElapsedMilliseconds);

        //    //TODO: TBD: also determine a way to verify Remaining?
        //}

        /// <summary>
        /// Returns whether <paramref name="a"/> Equals <paramref name="b"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override bool Equals(TimerRequest a, TimerRequest b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Implement this method in order to expose the prefix <see cref="SimulationTimer.op_Increment"/>
        /// operator. Technically the language handles this for us, and it should, but this gives us a code
        /// level handshake that indeed the operator was available and it did.
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/36x43w8w.aspx"> ++ Operator (C# Reference) </a>
        protected override SimulationTimer PrefixIncrementOperator(SimulationTimer timer)
        {
            return ++timer;
        }

        /// <summary>
        /// Implement this method in order to expose the postfix <see cref="SimulationTimer.op_Increment"/>
        /// operator. Technically the language handles this for us, and it should, but this gives us a code
        /// level handshake that indeed the operator was available and it did.
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/36x43w8w.aspx"> ++ Operator (C# Reference) </a>
        protected override SimulationTimer PostfixIncrementOperator(SimulationTimer timer)
        {
            // ReSharper disable once RedundantAssignment
            return timer++;
        }

        /// <summary>
        /// Implement this method in order to expose the prefix <see cref="SimulationTimer.op_Decrement"/>
        /// operator. Technically the language handles this for us, and it should, but this gives us a code
        /// level handshake that indeed the operator was available and it did.
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/wc3z3k8c.aspx"> -- Operator (C# Reference) </a>
        protected override SimulationTimer PrefixDecrementOperator(SimulationTimer timer)
        {
            return --timer;
        }

        /// <summary>
        /// Implement this method in order to expose the postfix <see cref="SimulationTimer.op_Decrement"/>
        /// operator. Technically the language handles this for us, and it should, but this gives us a code
        /// level handshake that indeed the operator was available and it did.
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/wc3z3k8c.aspx"> -- Operator (C# Reference) </a>
        protected override SimulationTimer PostfixDecrementOperator(SimulationTimer timer)
        {
            // ReSharper disable once RedundantAssignment
            return timer--;
        }

        /// <summary>
        /// Implement this method in order to expose the <see cref="SimulationTimer.op_Addition"/>
        /// assignment operator. Technically the language handles this for us, and it should, but this gives
        /// us a code level handshake that indeed the operator was available and it did.
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/sa7629ew.aspx"> += Operator (C# Reference) </a>
        protected override SimulationTimer AdditionAssignmentOperator(SimulationTimer timer, int steps)
        {
            return timer += steps;
        }

        /// <summary>
        /// Implement this method in order to expose the <see cref="SimulationTimer.op_Subtraction"/>
        /// assignment operator. Technically the language handles this for us, and it should, but this gives
        /// us a code level handshake that indeed the operator was available and it did.
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/2y9zhhx1.aspx"> -= Operator (C# Reference) </a>
        protected override SimulationTimer SubtractionAssignmentOperator(SimulationTimer timer, int steps)
        {
            return timer -= steps;
        }

        #endregion
    }
}
