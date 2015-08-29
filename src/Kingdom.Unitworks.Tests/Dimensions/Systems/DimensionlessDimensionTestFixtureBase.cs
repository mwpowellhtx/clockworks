namespace Kingdom.Unitworks.Dimensions.Systems
{
    public abstract class DimensionlessDimensionTestFixtureBase<TDimension, TInterface>
        : DimensionTestFixtureBase<TDimension, TInterface>
        where TDimension : class, TInterface
        where TInterface : IDimensionless
    {
    }
}
