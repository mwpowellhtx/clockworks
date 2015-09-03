namespace Kingdom.Unitworks.Dimensions
{
    using Systems.SI;

    /// <summary>
    /// Like its two dimensional counterpart, <see cref="IPlanarAngle"/>, the Solid Angle
    /// represents the Conal area cut out from an angle in three dimensional space. It is
    /// also unitless, although, technically, it can be considered <see cref="Area.SquareMeter"/>
    /// per SquareMeter.
    /// </summary>
    /// <a href="!:http://en.wikipedia.org/wiki/Solid_angle" >Solid angle</a>
    public interface ISolidAngle : IDerivedDimension
    {
    }
}
