namespace Kingdom.Unitworks.Dimensions.Systems
{
    public abstract class BaseDimensionTestFixtureBase<TDimension, TInterface>
        : DimensionTestFixtureBase<TDimension, TInterface>
        where TDimension : class, TInterface
        where TInterface : IBaseDimension
    {
    }
}
