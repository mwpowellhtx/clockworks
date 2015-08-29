namespace Kingdom.Unitworks
{
    //TODO: put in a crap ton of Quantity Comparable, Equatable, and logical operator unit tests... with a host of homogenous and heterogeneous use cases... invocation methods, etc, abound ...
    public abstract class QuantityTestFixtureBase : TestFixtureBase
    {
        protected virtual IQuantity A { get; set; }

        protected abstract void InitializeQuantities();

        public override void SetUp()
        {
            base.SetUp();

            InitializeQuantities();
        }

        protected abstract OperatorPart Operator { get; }
    }
}
