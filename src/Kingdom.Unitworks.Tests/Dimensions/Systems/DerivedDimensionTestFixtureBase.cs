namespace Kingdom.Unitworks.Dimensions.Systems
{
    public abstract class DerivedDimensionTestFixtureBase<TDimension, TInterface>
        : DimensionTestFixtureBase<TDimension, TInterface>
        where TDimension : class, TInterface
        where TInterface : IDerivedDimension
    {
    }
}
