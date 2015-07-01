using System;

namespace Kingdom.Unitworks
{
    /// <summary>
    /// This is a place holder that signals that a unit enum is the base unit.
    /// There must be one such unit flagged with this attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class BaseUnitAttribute : Attribute
    {
    }
}
