using System;

namespace Kingdom.Unitworks.Trigonometric
{
    internal interface ITrigonometricPart : IDisposable
    {
        IQuantity Qty { get; }
    }
}
