namespace Kingdom.Unitworks.Dimensions.Systems
{
    /// <summary>
    /// Represents a base dimension test fixture base class.
    /// </summary>
    /// <typeparam name="TDimension"></typeparam>
    /// <typeparam name="TInterface"></typeparam>
    public abstract class BaseDimensionTestFixtureBase<TDimension, TInterface>
        : DimensionTestFixtureBase<TDimension, TInterface>
        where TDimension : class, TInterface
        where TInterface : IBaseDimension
    {
    }
}
