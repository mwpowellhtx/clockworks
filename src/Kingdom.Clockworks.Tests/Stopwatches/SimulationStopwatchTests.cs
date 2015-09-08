using System.Linq;
using Kingdom.Unitworks;

namespace Kingdom.Clockworks.Stopwatches
{
    using T = Unitworks.Dimensions.Systems.Commons.Time;

    public class SimulationStopwatchTests : TimeableClockTestFixtureBase<
        SimulationStopwatch, StopwatchRequest, StopwatchElapsedEventArgs>
    {
        protected override IQuantity CalculateEstimated(ChangeType change, int steps,
            IQuantity intervalTimePerTimeQty, IQuantity timePerStepQty)
        {
            var resultQty = (Quantity) intervalTimePerTimeQty*timePerStepQty*steps;

            var changes = new[] {ChangeType.Decrement, ChangeType.Subtraction};

            if (changes.Any(x => (change & x) == x))
                resultQty = -resultQty;

            ////TODO: same here: for now we will allow negative... i.e. counting up from play clock (negative_ to time of possession (positive)
            //// Cannot be less than Zero.
            //resultQty = (Quantity) Quantity.Max(Quantity.Zero(T.Millisecond), resultQty);

            return resultQty;
        }
    }
}
