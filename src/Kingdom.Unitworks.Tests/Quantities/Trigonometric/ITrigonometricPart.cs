using System;

namespace Kingdom.Unitworks.Trigonometric
{
    /// <summary>
    /// Represents a part of the trigonometric calculations and unit tests.
    /// </summary>
    internal interface ITrigonometricPart : IDisposable
    {
        IQuantity Qty { get; }
    }
}
