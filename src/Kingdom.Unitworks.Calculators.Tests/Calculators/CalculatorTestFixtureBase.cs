namespace Kingdom.Unitworks.Calculators
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TCalculator"></typeparam>
    public abstract class CalculatorTestFixtureBase<TCalculator> : TestFixtureBase
        where TCalculator : class, ICalculator, new()
    {
        protected TCalculator Calculator { get; private set; }

        public override void SetUp()
        {
            base.SetUp();

            Calculator = new TCalculator();
        }

        public override void TearDown()
        {
            Calculator = null;

            base.TearDown();
        }
    }
}
