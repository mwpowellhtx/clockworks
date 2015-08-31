using System.Linq;
using Kingdom.Unitworks;

namespace Kingdom.Clockworks.Timers
{
    using T = Unitworks.Dimensions.Systems.Commons.Time;

    public class SimulationTimerTests : TimeableClockTestFixtureBase<
        SimulationTimer, TimerRequest, TimerElapsedEventArgs>
    {
        protected override IQuantity CalculateEstimated(ChangeType change, int steps,
            IQuantity intervalTimePerTimeQty, IQuantity timePerStepQty)
        {
            var resultQty = (Quantity) intervalTimePerTimeQty*timePerStepQty*steps;

            // Timers "increment" in the opposite direction, but the time away from zero is still the same quantity.
            var changes = new[] {ChangeType.Increment, ChangeType.Addition};

            if (changes.Any(x => (change & x) == x))
                resultQty = -resultQty;

            ////TODO: for now, we will allow negative.
            //// Cannot be less than Zero.
            //resultQty = (Quantity) Quantity.Max(Quantity.Zero(T.Millisecond), resultQty);

            return resultQty;
        }
    }
}

////TODO: rethink the tests on account of massive simplification of the quantities involved...
//using Kingdom.Unitworks;
//using Kingdom.Unitworks.Units;

//namespace Kingdom.Clockworks.Timers
//{
//    using T = Unitworks.Dimensions.Systems.Commons.Time;

//    /// <summary>
//    /// Extends the <see cref="TimeableClockTestFixtureBase{TClock, TRequest, TElapsedEventArgs}"/>
//    /// to support the <see cref="SimulationTimer"/> and supporting cast of types.
//    /// </summary>
//    public class SimulationTimerTests
//        : TimeableClockTestFixtureBase<
//            SimulationTimer
//            , TimerRequest
//            , TimerElapsedEventArgs>
//    {
//        /// <summary>
//        /// Returns a created <see cref="TimerRequest"/> given the arguments.
//        /// </summary>
//        /// <param name="direction"></param>
//        /// <param name="millisecondsPerStep"></param>
//        /// <param name="steps"></param>
//        /// <param name="type"></param>
//        /// <returns></returns>
//        protected override TimerRequest MakeRequest(RunningDirection? direction = null,
//            double millisecondsPerStep = 1000d, int steps = 1,
//            RequestType type = RequestType.Instantaneous)
//        {
//            return new TimerRequest(direction, new Quantity(millisecondsPerStep, T.Millisecond), steps, type);
//        }

//        /// <summary>
//        /// Performs the arrangement and actions supporting the
//        /// <see cref="TimeableClockTestFixtureBase{SimulationTimer, TimerRequest, TimerElapsedEventArgs}.Elapsed_events_yield_expected_event_counts"/>
//        /// tests.
//        /// </summary>
//        /// <param name="timer"></param>
//        /// <param name="durationSeconds"></param>
//        protected override void MakeElapsedEventYieldArrangements(SimulationTimer timer, double durationSeconds)
//        {
//            // Should be well within whatever duration we were given.
//            timer[T.Second, T.Second] = 0.1d;

//            // Reset the durations.
//            timer.Reset(durationSeconds);
//        }

//        #region Increment and Decrement Members

//        /// <summary>
//        /// Initializes <paramref name="theTimer"/> given the arguments.
//        /// </summary>
//        /// <param name="theTimer"></param>
//        /// <param name="intervalSecondsPerSecond"></param>
//        /// <param name="millisecondsPerStep"></param>
//        protected override void InitializeStarNcrementClock(SimulationTimer theTimer,
//            double intervalSecondsPerSecond, double millisecondsPerStep)
//        {
//            base.InitializeStarNcrementClock(theTimer, intervalSecondsPerSecond,
//                millisecondsPerStep);

//            // Remember to reset in terms of milliseconds.
//            var intervalQuantity = intervalSecondsPerSecond.ToTimeQuantity()
//                .ToTimeQuantity(TimeUnit.Millisecond);

//            theTimer.TargetMilliseconds = (intervalQuantity*10d).Value;
//        }

//        ///// <summary>
//        ///// 
//        ///// </summary>
//        ///// <param name="e"></param>
//        ///// <param name="expectedElapsedMilliseconds"></param>
//        //protected override void VerifyElapsedEventResults(TimerElapsedEventArgs e, double expectedElapsedMilliseconds)
//        //{
//        //    base.VerifyElapsedEventResults(e, expectedElapsedMilliseconds);

//        //    //TODO: TBD: also determine a way to verify Remaining?
//        //}

//        /// <summary>
//        /// Returns whether <paramref name="a"/> Equals <paramref name="b"/>.
//        /// </summary>
//        /// <param name="a"></param>
//        /// <param name="b"></param>
//        /// <returns></returns>
//        protected override bool Equals(TimerRequest a, TimerRequest b)
//        {
//            return a.Equals(b);
//        }

//        /// <summary>
//        /// Implement this method in order to expose the prefix <see cref="SimulationTimer.op_Increment"/>
//        /// operator. Technically the language handles this for us, and it should, but this gives us a code
//        /// level handshake that indeed the operator was available and it did.
//        /// </summary>
//        /// <param name="timer"></param>
//        /// <returns></returns>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/36x43w8w.aspx"> ++ Operator (C# Reference) </a>
//        protected override SimulationTimer PrefixIncrementOperator(SimulationTimer timer)
//        {
//            return ++timer;
//        }

//        /// <summary>
//        /// Implement this method in order to expose the postfix <see cref="SimulationTimer.op_Increment"/>
//        /// operator. Technically the language handles this for us, and it should, but this gives us a code
//        /// level handshake that indeed the operator was available and it did.
//        /// </summary>
//        /// <param name="timer"></param>
//        /// <returns></returns>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/36x43w8w.aspx"> ++ Operator (C# Reference) </a>
//        protected override SimulationTimer PostfixIncrementOperator(SimulationTimer timer)
//        {
//            // ReSharper disable once RedundantAssignment
//            return timer++;
//        }

//        /// <summary>
//        /// Implement this method in order to expose the prefix <see cref="SimulationTimer.op_Decrement"/>
//        /// operator. Technically the language handles this for us, and it should, but this gives us a code
//        /// level handshake that indeed the operator was available and it did.
//        /// </summary>
//        /// <param name="timer"></param>
//        /// <returns></returns>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/wc3z3k8c.aspx"> -- Operator (C# Reference) </a>
//        protected override SimulationTimer PrefixDecrementOperator(SimulationTimer timer)
//        {
//            return --timer;
//        }

//        /// <summary>
//        /// Implement this method in order to expose the postfix <see cref="SimulationTimer.op_Decrement"/>
//        /// operator. Technically the language handles this for us, and it should, but this gives us a code
//        /// level handshake that indeed the operator was available and it did.
//        /// </summary>
//        /// <param name="timer"></param>
//        /// <returns></returns>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/wc3z3k8c.aspx"> -- Operator (C# Reference) </a>
//        protected override SimulationTimer PostfixDecrementOperator(SimulationTimer timer)
//        {
//            // ReSharper disable once RedundantAssignment
//            return timer--;
//        }

//        /// <summary>
//        /// Implement this method in order to expose the <see cref="SimulationTimer.op_Addition"/>
//        /// assignment operator. Technically the language handles this for us, and it should, but this gives
//        /// us a code level handshake that indeed the operator was available and it did.
//        /// </summary>
//        /// <param name="timer"></param>
//        /// <param name="steps"></param>
//        /// <returns></returns>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/sa7629ew.aspx"> += Operator (C# Reference) </a>
//        protected override SimulationTimer AdditionAssignmentOperator(SimulationTimer timer, int steps)
//        {
//            return timer += steps;
//        }

//        /// <summary>
//        /// Implement this method in order to expose the <see cref="SimulationTimer.op_Subtraction"/>
//        /// assignment operator. Technically the language handles this for us, and it should, but this gives
//        /// us a code level handshake that indeed the operator was available and it did.
//        /// </summary>
//        /// <param name="timer"></param>
//        /// <param name="steps"></param>
//        /// <returns></returns>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/2y9zhhx1.aspx"> -= Operator (C# Reference) </a>
//        protected override SimulationTimer SubtractionAssignmentOperator(SimulationTimer timer, int steps)
//        {
//            return timer -= steps;
//        }

//        #endregion
//    }
//}
