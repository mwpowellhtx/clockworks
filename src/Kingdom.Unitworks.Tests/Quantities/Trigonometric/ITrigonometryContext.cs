using System;

namespace Kingdom.Unitworks.Trigonometric
{
    internal interface ITrigonometryContext : IDisposable
    {
        ITrigonometryContext SetEpsilon(double value);

        bool HasEpsilon { get; }

        double Epsilon { get; }

        ITrigonometryContext Starting(Func<ITrigonometryContext, ITrigonometricPart> starting);

        ITrigonometryContext Expected(Func<ITrigonometryContext, ITrigonometricPart> expected);

        ITrigonometryContext Function(Func<IQuantity, IQuantity> function);
    }
}
