namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// Technically, according to the SI unit system, an Angle is Length per Length,
    /// or more specifically, <see cref="Systems.SI.Length.Meter"/> per Meter, which yields
    /// dimensionless. Technically that means it is a derived, or compound, unit.
    /// But due to constraints of good object oriented design, it is better represented
    /// as a unary dimension.
    /// </summary>
    /// <a href="!:http://en.wikipedia.org/wiki/SI_derived_unit" > SI_derived_unit </a>
    /// <a href="!:http://en.wikipedia.org/wiki/Angle" > Angle </a>
    /// <a href="!:http://en.wikipedia.org/wiki/Radian" > Radian </a>
    public interface IPlanarAngle : IBaseDimension, IDimensionless
    {
    }
}
